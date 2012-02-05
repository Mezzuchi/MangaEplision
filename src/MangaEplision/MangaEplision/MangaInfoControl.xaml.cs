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
using System.Windows.Media.Effects;
using MangaEplision.Base;
using MangaEplision.Metro;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for ArrowedPopupControl.xaml
    /// </summary>
    public partial class MangaInfoControl : UserControl
    {
        public MangaInfoControl(MangaEplision.Base.Manga info)
        {
            InitializeComponent();

            this.DataContext = info;
            //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private void downloadTile_Click(object sender, RoutedEventArgs e)
        {
            if (BooksLsView.SelectedItem == null)
                MessageBox.Show("Please select a book before pressing this button!");
            else
            {
                BookEntry be = (BookEntry)BooksLsView.SelectedItem;

                if (Global.GetBookExist((Manga)this.DataContext, be))
                    MessageBox.Show("This book is already downloaded!");
                else
                {
                    IndefiniteProgressDialog ipd = new IndefiniteProgressDialog();
                    ipd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ipd.Topmost = true;
                    Dispatcher.Invoke(new EmptyDelegate(() =>
                        ipd.Show()));
                    ((MainWindow)Application.Current.MainWindow).metroTabControl1.SelectedIndex = 1;
                    Global.DownloadMangaBook(
                                        (Manga)this.DataContext, be, () =>
                                        {
                                            Dispatcher.Invoke(new EmptyDelegate(() =>
                                                {
                                                    var collectionTab = ((MetroTabItem)((MainWindow)Application.Current.MainWindow).metroTabControl1.Items[1]);
                                                    collectionTab.UpdateLayout();
                                                    Global.LoadCollection();
                                                    var ls = ((ListView)((Grid)collectionTab.Content).FindName("CollectionListView"));
                                                    try
                                                    {
                                                        ls.GetBindingExpression(ListView.ItemsSourceProperty).UpdateTarget();
                                                    }
                                                    catch (Exception)
                                                    {
                                                        var source = ls.ItemsSource;
                                                        ls.ItemsSource = null;
                                                        ls.ItemsSource = Global.BookCollection;
                                                    }
                                                    ipd.Close();
                                                }
                                                ));
                                        });
                    //
                }
            }
        }
    }
}
