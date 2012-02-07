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
using System.Windows.Threading;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        internal List<QueueItem> DlQueue;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.StateChanged += new EventHandler(MainWindow_StateChanged);
            this.SizeChanged += new SizeChangedEventHandler(MainWindow_SizeChanged);
            DlQueue = new List<QueueItem>();
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (metroBanner.Visibility == System.Windows.Visibility.Collapsed)
            CatalogListBox.Height = (this.Height - (this.Height - metroTabControl1.ActualHeight)) - DashboardTab.ActualHeight * 2;
            else if (metroBanner.Visibility == System.Windows.Visibility.Visible) ;
            CatalogListBox.Height = (this.Height - (this.Height - metroTabControl1.ActualHeight)) - DashboardTab.ActualHeight * 2 - metroBanner.ActualHeight;
        }


        void MainWindow_StateChanged(object sender, EventArgs e)
        {


        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(1000);
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
                            if (Global.SavedQueue())
                            {
                                Global.LoadQueue(ref DlQueue);
                                DlQueueList.ItemsSource = DlQueue;
                                DlQueueList.Items.Refresh();
                                QueueStatuslbl.Content = string.Format("There Are currently {0} Items in your Queue", DlQueue.Count);
                                if (DlQueue.Count > 0)
                                    CallStartQueueProcess();

                                Global.CleanupQueueDir();
                            }
                            if (NetworkUtils.IsConnectedToInternet())
                            {
                                foreach (BookEntry be in Global.MangaSource.GetNewReleasesOfToday())
                                {
                                    var slide = new MetroBannerSlide();
                                    slide.Header = be.Name + " / " + be.ParentManga.MangaName;
                                    slide.Image = new BitmapImage(new Uri(be.ParentManga.BookImageUrl));
                                    metroBanner.Slides.Add(slide);
                                }
                                metroBanner.Slide = metroBanner.Slides[0];
                                metroBanner.Start();
                            }
                        }));
                }).ContinueWith((task) =>
                    {
                        Dispatcher.Invoke(
                            new EmptyDelegate(() =>
                                {
                                    try
                                    {
                                        if (NetworkUtils.IsConnectedToInternet())
                                            metroBanner.Visibility = System.Windows.Visibility.Visible;
                                        else
                                            metroBanner.Visibility = System.Windows.Visibility.Collapsed;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }));
                    });
            Application.Current.Exit += new ExitEventHandler((o, er) => { Global.Current_Exit(o, er); });
            this.Closing += new System.ComponentModel.CancelEventHandler((s, er) => { Global.SaveQueue(this.DlQueue); });
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Global.Current_DispatcherUnhandledException);


        }
        private delegate void EmptyDelegate();
        private delegate void UpdateDelegate(int Current, int Total);
        private delegate void QueueUpDelegate(QueueItem q);
        private void viewInfoTile_Click(object sender, RoutedEventArgs e)
        {
            if (CatalogListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have to select a name before I can fetch information about it!");
            }
            else
            {
                if (NetworkUtils.IsConnectedToInternet())
                {
                    Manga info = null;
                    string name = CatalogListBox.SelectedItem.ToString();
                    var task = Task.Factory.StartNew(() =>
                        {
                            info = Global.GetMangaInfo(name);
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
                else
                {
                    MessageBox.Show("You are not connected to the internet! Connect and try again!");
                }
            }
        }

        private void CollectionListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CollectionListView.SelectedItem != null)
            {
                var mr = new MangaReaderWin();
                mr.DataContext = CollectionListView.SelectedItem;
                mr.Show();
            }
        }

        internal void AddToQueue(BookEntry bookEntry, MangaEplision.Base.Manga manga)
        {
            if (DlQueue != null)
            {
                QueueItem qi = new QueueItem(bookEntry, manga);
                if (!DlQueue.Contains(qi) && !Global.GetBookExist(qi.Manga, qi.Book))
                {
                    DlQueue.Add(qi);
                    DlQueueList.ItemsSource = DlQueue;
                    DlQueueList.Items.Refresh();
                    QueueStatuslbl.Content = string.Format("There Are currently {0} Items in your Queue", DlQueue.Count);
                }
            }
        }

        internal void CallStartQueueProcess()
        {
            var queueRunner = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                int i = 0;
                while (DlQueue.Count > 0)
                {
                    QueueItem q = DlQueue[i];
                    if (Global.GetBookExist(q.Manga, q.Book))
                        return;
                    else
                    {
                        q.Downloading = true;
                        q.Status = QueueStatus.Downloading;
                        Dispatcher.Invoke(new QueueUpDelegate((qi) =>
                                        {
                                            QueueStatuslbl.Content = string.Format("Downloading {1}", DlQueue.Count, q.Name);
                                            DlQueueList.ItemsSource = DlQueue;
                                            DlQueueList.Items.Refresh();
                                        }), q);
                        Global.DownloadMangaBook(q.Manga, q.Book, () =>
                        {
                            q.Downloading = false; q.Status = QueueStatus.Completed; DlQueue.Remove(q); Dispatcher.Invoke(new EmptyDelegate(() =>
                            {
                                DlQueueList.ItemsSource = DlQueue;
                                DlQueueList.Items.Refresh();
                                QueueStatuslbl.Content = string.Format("Done! {0} Items Left in Queue", DlQueue.Count);
                                CurrProg.Value = 0;
                                Count.Content = string.Format("{0}%", 0);
                            }));
                        }, (curr, total) =>
                        {
                            Dispatcher.Invoke(new UpdateDelegate((cur, tot) =>
                            {
                                int precent = ((((cur < tot) ? cur + 1 : cur) * 100) / tot);
                                CurrProg.Value = precent;
                                Count.Content = string.Format("{0}%", precent);
                            }), curr, total);
                        });
                        while (q.Downloading)
                            System.Threading.Thread.Sleep(30000);
                    }
                }
            });
        }

        private void searchTile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchTile_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
