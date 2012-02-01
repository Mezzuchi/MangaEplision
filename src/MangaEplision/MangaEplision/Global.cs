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
        public static void Initialize()
        {
            DataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MangaEplision";

            if (!Directory.Exists(DataDir))
                Directory.CreateDirectory(DataDir);

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
    }
}
