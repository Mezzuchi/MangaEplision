using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MangaEplision.Metro;
using System.Threading.Tasks;
using MangaEplision.Base;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : MetroWindow
    {
        private bool Move = false;
        private delegate void EmptyDelegate();
        private delegate void UpdateDelegate(int Current, int Total);
        private delegate void CompeatedDelegate(object item, bool all = false);
        public ImportWindow()
        {
            InitializeComponent();
        }

        private void ImportTile_Click(object sender, RoutedEventArgs e)
        {
            this.Move = (bool)cbMv.IsChecked;
            var task = Task.Factory.StartNew(() =>
                       {
                           if (!System.IO.Directory.Exists(Global.CollectionDir + "Imports"))
                               System.IO.Directory.CreateDirectory(Global.CollectionDir + "Imports");
                           Dispatcher.Invoke(new EmptyDelegate(() =>
                           {
                               this.pbar.Value = 0;
                           }));
                           int i = 0;
                           int tot = ImportList.Items.Count;
                           while (ImportList.Items.Count > 0)
                           {
                               string name = string.Empty;
                               string path = ImportList.Items[i].ToString();
                               name = (new FileInfo(System.IO.Path.GetDirectoryName(path))).Directory.Name;
                               if (!System.IO.Directory.Exists(Global.CollectionDir + "Imports\\" + name))
                               {
                                   System.IO.Directory.CreateDirectory(Global.CollectionDir + "Imports\\" + name);
                                   Manga manga = new Manga();
                                   manga.Author = "Unknown";
                                   manga.Description = "Unknown";
                                   manga.MangaName = name;
                                   if (!Move)
                                   {
                                       Book b = new Book(manga);
                                       b.Name = name;
                                       b.Filename = name + ".bin";
                                       foreach (var file in System.IO.Directory.EnumerateFiles(path))
                                       {
                                           File.Copy(file, Global.CollectionDir + "Imports\\" + name + "\\" + System.IO.Path.GetFileName(file));
                                           b.PageLocalUrls.Add(Global.CollectionDir + "Imports\\" + name + "\\" + System.IO.Path.GetFileName(file));
                                       }
                                       using (var fs = new FileStream(Global.CollectionDir + "Imports\\" + name + "\\" + b.Filename, FileMode.Open))
                                       {
                                           BinaryFormatter bf = new BinaryFormatter();
                                           bf.Serialize(fs, b);
                                           fs.Close();
                                       }
                                   }
                               }
                               Dispatcher.Invoke(new UpdateDelegate((Cur, Tot) =>
                               {
                                   int pre = ((Cur * 100) / Tot);
                                   this.pbar.Value = pre;
                               }), i, tot);
                           }
                           Dispatcher.Invoke(new EmptyDelegate(() =>
                           {
                               this.pbar.Value = 0;
                           }));
                       });
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
           var fb = new System.Windows.Forms.FolderBrowserDialog();
           fb.Description = "Find Manga Folder";
           fb.ShowNewFolderButton = false;
           if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           {
               if (!(bool)cbLook.IsChecked)
                   if (!ImportList.Items.Contains(fb.SelectedPath))
                       ImportList.Items.Add(fb.SelectedPath);
                   else
                       MessageBox.Show("This Item is Already in the Import List!", "Import Manga", MessageBoxButton.OK);
               else
               {
                   foreach (var MangaDir in System.IO.Directory.EnumerateDirectories(fb.SelectedPath))
                   {
                       if (!ImportList.Items.Contains(MangaDir))
                           ImportList.Items.Add(MangaDir);
                   }
               }
           }
           fb.Dispose();
        }
    }
}
