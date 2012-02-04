// -----------------------------------------------------------------------
// <copyright file="Global.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MangaEplision
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows;
    using System.Threading.Tasks;
using MangaEplision.Base;

    public static class Global
    {
        public static string DataDir = null;
        public static string CatalogFilename = null;
        public static string CollectionDir = null;
        public static void Initialize()
        {
            DataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MangaEplision";

            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);

            CollectionDir = DataDir + "\\Collection\\";

            if (!Directory.Exists(CollectionDir))
                Directory.CreateDirectory(CollectionDir);

            fswatch = new FileSystemWatcher(CollectionDir);

            fswatch.EnableRaisingEvents = true;

            fswatch.Changed += new FileSystemEventHandler(fswatch_Changed);

            fswatch.Deleted += new FileSystemEventHandler(fswatch_Deleted);

            fswatch.Created += new FileSystemEventHandler(fswatch_Created);

            CollectionBooks = new List<Book>();

            LoadCollection();

            CatalogFilename = DataDir + "\\Catalog.bin";

            if (!File.Exists(CatalogFilename))
            {
                if (MessageBox.Show("I could not find a manga catalog stored on your computer. I will need to download one. Is that okay?", "Catalog Not Found", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IndefiniteProgressDialog prog = null;

                    Application.Current.Dispatcher.Invoke(new EmptyDelegate(() =>
                        {
                            prog = new IndefiniteProgressDialog();
                            prog.Owner = Application.Current.Windows[0];
                            prog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            prog.Topmost = true;
                            prog.Show();
                        }));

                    Task.Factory.StartNew(() =>
                    {
                        BinaryFormatter binf = new BinaryFormatter();

                        LoadSource();

                        var fs = new FileStream(CatalogFilename, FileMode.Create);
                        var dict = MangaSource.GetAvailableManga();
                        binf.Serialize(fs, dict);
                        fs.Close();
                        fs.Dispose();
                    }).ContinueWith((obj) =>
                    {
                        Application.Current.Dispatcher.Invoke(
                            new EmptyDelegate(() =>
                            {
                                prog.Close();
                            }));
                        LoadCatalog();
                    }).Wait();
                }
                else
                {
                    MessageBox.Show("You will not be able to read manga unless you download the catalog!");
                    LoadSource();
                }

            }
            else
            {
                LoadCatalog();
            }
        }

        #region Collection
        static void fswatch_Created(object sender, FileSystemEventArgs e)
        {
            using (var fs = new FileStream(e.FullPath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    Book b = (Book)bf.Deserialize(fs);
                    b.Filename = e.FullPath;
                    CollectionBooks.Add(b);
                }
                catch (Exception)
                {

                }
                finally
                {
                    fs.Close();
                }
            }

            BookCollection = CollectionBooks;
        }

        static void fswatch_Changed(object sender, FileSystemEventArgs e)
        {
            
        }

        static void fswatch_Deleted(object sender, FileSystemEventArgs e)
        {
            Book bk = null;
            foreach (Book b in CollectionBooks)
                if (b.Filename == e.FullPath)
                {
                    bk = b;
                    break;
                }
            if (bk != null)
                CollectionBooks.Remove(bk);
            BookCollection = CollectionBooks;

        }

        private static void LoadCollection()
        {
            BinaryFormatter bf = new BinaryFormatter();

            foreach (string file in Directory.GetFiles(CollectionDir, "*.bin"))
            {
                var fs = new FileStream(file,FileMode.Open);
                try
                {
                    Book b = (Book)bf.Deserialize(fs);
                    b.Filename = file;
                    CollectionBooks.Add(b);

                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to load book from collection!" + Environment.NewLine + "File: " + file);
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            BookCollection = CollectionBooks;
            
        }

        private static FileSystemWatcher fswatch = null;

        public static List<Book> CollectionBooks = null;
        public static List<Book> BookCollection
        {
            get
            {
                return (List<Book>)Application.Current.Dispatcher.Invoke(new EmptyReturnDelegate(() =>
                    (List<Book>)Application.Current.MainWindow.GetValue(BookCollectionProperty)));
            }
            set 
            {
                Application.Current.Dispatcher.Invoke(
                    new EmptyDelegate(
                        () => 
                            {
                                Application.Current.MainWindow.SetValue(BookCollectionProperty, value);

                                //Screw MVVM at this point:
                                ((MainWindow)Application.Current.MainWindow).CollectionListView.ItemsSource = value;
                            }));
            }
        }
        public static readonly DependencyProperty BookCollectionProperty = DependencyProperty.Register("BookCollection", typeof(List<Book>), typeof(Global));
        #endregion

        private delegate object EmptyReturnDelegate();
        private delegate void EmptyDelegate();
        private static void LoadSource()
        {
            MangaSource = new MangaEplision.Sources.MangaReader.MangaReaderSource(true);
        }
        private static void LoadCatalog()
        {
            BinaryFormatter binf = new BinaryFormatter();

            var fs = new FileStream(CatalogFilename, FileMode.Open);
            MangaDictionary = (Dictionary<string, string>)binf.Deserialize(fs);
            Mangas = MangaDictionary.Keys.ToArray();

            if (MangaSource == null)
                MangaSource = new MangaEplision.Sources.MangaReader.MangaReaderSource(MangaDictionary);

            fs.Close();
            fs.Dispose();
        }
        public static string[] Mangas { get; private set; }
        public static Dictionary<string, string> MangaDictionary { get; private set; }
        public static IMangaSource MangaSource { get; private set; }
        public static bool GetBookExist(Manga manga, BookEntry book)
        {
            var firstcheck = File.Exists(CollectionDir + "\\" + manga.MangaName + "_" + book.Name.Replace(":","-") + ".bin");

            if (firstcheck)
                return true;
            else
            {
                foreach (var b in CollectionBooks)
                    if (b.Name == book.Name)
                        return true;
            }

            return false;
        }
        public static void DownloadMangaBook(Manga manga, BookEntry book, Action act = null)
        {
            Task.Factory.StartNew(() =>
                {
                    Book bk = Global.MangaSource.GetBook(manga, book);

                    BinaryFormatter bf = new BinaryFormatter();
                    using (var fs = new FileStream(CollectionDir + "\\" + bk.ParentManga.MangaName + "_" + bk.Name.Replace(":","-") + ".bin", FileMode.Create))
                    {
                        try
                        {
                            bf.Serialize(fs, bk);
                        }
                        catch (Exception)
                        {
                            
                        }
                        fs.Close();
                    }
                }).ContinueWith(
                (TSK) =>
                {
                    if (act != null)
                        act();

                    return TSK.Exception;
                });

        }
    }
    public delegate void EmptyDelegate();
}
