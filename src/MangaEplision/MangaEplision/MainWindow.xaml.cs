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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MangaEplision.Metro;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using MangaEplision.Base;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.StateChanged += new EventHandler(MainWindow_StateChanged);

            winState = System.Windows.WindowState.Normal;
            winNormalCTLBSize = CatalogListBox.Height;

            winMaxCTLBSize = metroTabControl1.ActualHeight + 100;
        }

        private WindowState winState;
        private double winNormalCTLBSize = 0;
        private double winMaxCTLBSize = 0;
        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                CatalogListBox.Height = winNormalCTLBSize;

                winState = System.Windows.WindowState.Normal;
            }
            else if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                CatalogListBox.Height = winMaxCTLBSize;

                winState = System.Windows.WindowState.Maximized;
            }
            else
            {
                if (winState == System.Windows.WindowState.Normal)
                {
                    CatalogListBox.Height = metroTabControl1.ActualHeight - 300;

                    winState = System.Windows.WindowState.Normal;
                }
                else if (winState == System.Windows.WindowState.Maximized)
                {
                    CatalogListBox.Height = metroTabControl1.ActualHeight + 100;

                    winState = System.Windows.WindowState.Maximized;
                }
            }

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(2000);
                    Global.Initialize();

                    Dispatcher.Invoke(
                        new EmptyDelegate(
                        () =>
                        {
                            if (Global.Mangas != null)
                            {
                                CatalogListBox.ItemsSource = Global.Mangas;
                                metroGroupBox1.NotificationsCount = Global.Mangas.Length;
                            }
                        }));
                });
        }
        private delegate void EmptyDelegate();

        private void viewInfoTile_Click(object sender, RoutedEventArgs e)
        {
            if (CatalogListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have to select a name before I can fetch information about it!");
            }
            else
            {
                Manga info = null;
                string name = CatalogListBox.SelectedItem.ToString();
                var task = Task.Factory.StartNew(() =>
                    {
                        info = Global.MangaSource.GetMangaInfo(name);
                    }).ContinueWith((tsk) =>
                        {
                            if (tsk.Exception == null)
                                Dispatcher.Invoke(
                                    new EmptyDelegate(
                                        () =>
                                        {
                                            MetroTabItem mti = new MetroTabItem();
                                            mti.IsClosable = true;
                                            mti.Header = info.MangaName;
                                            mti.Content = new MangaInfoControl(info);

                                            metroTabControl1.Items.Add(mti);

                                            metroTabControl1.SelectedItem = mti;
                                        }));
                            else
                                MessageBox.Show("There was an error grabbing information on that manga!" + Environment.NewLine + "Geeky error details: " + Environment.NewLine + Environment.NewLine + tsk.Exception.ToString() + Environment.NewLine + Environment.NewLine + "NOTE: You can press CTRL + C to copy the contents of this message!");
                        });
            }
        }
    }
}
