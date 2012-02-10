using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MangaEplision.Base;
using MangaEplision.Metro;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        internal bool QueueRunning = false;
        internal List<QueueItem> DlQueue;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.StateChanged += new EventHandler(MainWindow_StateChanged);
            this.SizeChanged += new SizeChangedEventHandler(MainWindow_SizeChanged);
            this.CatalogListBox.MouseDoubleClick += new MouseButtonEventHandler(CatalogListBox_MouseDoubleClick);
            DlQueue = new List<QueueItem>();
            
        }

        void CatalogListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewInfoTile_Click(sender, e);
        }

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (metroBanner.Visibility == System.Windows.Visibility.Collapsed)
                CatalogListBox.Height = (this.Height - ((this.Height - metroTabControl1.ActualHeight) - LatestReleaseGB.FontSize)) - DashboardTab.ActualHeight * 3;
            else if (metroBanner.Visibility == System.Windows.Visibility.Visible)
                CatalogListBox.Height = (this.Height - (this.Height - metroTabControl1.ActualHeight)) - DashboardTab.ActualHeight * 2 - metroBanner.ActualHeight - LatestReleaseGB.FontSize;
        }


        void MainWindow_StateChanged(object sender, EventArgs e)
        {


        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
            {
                //if Win7
                this.TaskbarItemInfo = new System.Windows.Shell.TaskbarItemInfo();
            }


            var mainTsk = Task.Factory.StartNew(() =>
                {
                    #region
                    System.Threading.Thread.Sleep(100);
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

                    #endregion
                        }));
                }).ContinueWith((task) =>
                    {
                        var t1 = Task.Factory.StartNew(() =>
                        {
                            if (Global.SavedQueue())
                            {
                                Global.LoadQueue(ref DlQueue);
                                Dispatcher.Invoke(new EmptyDelegate(() =>
                                {
                                    DlQueueList.ItemsSource = DlQueue;
                                    DlQueueList.Items.Refresh();
                                    QueueStatuslbl.Content = string.Format("There are currently {0} items in your queue.", DlQueue.Count);
                                    if (DlQueue.Count > 0)
                                        CallStartQueueProcess();
                                }));
                                Global.CleanupQueueDir();
                            }
                        });

                        var t2 = Task.Factory.StartNew(() =>
                        {
                            if (NetworkUtils.IsConnectedToInternet())
                            {
                                foreach (BookEntry be in Global.MangaSource.GetNewReleasesOfToday(3))
                                {

                                    Dispatcher.BeginInvoke(new EmptyDelegate(
                                        () =>
                                        {
                                            var bitmp = new BitmapImage();

                                            bitmp.BeginInit();
                                            bitmp.CacheOption = BitmapCacheOption.Default;


                                            bitmp.UriSource = new Uri(be.ParentManga.BookImageUrl);
                                            bitmp.EndInit();

                                            //bitmp.Freeze();
                                            var slide = new MetroBannerSlide();
                                            slide.Header = be.Name + " / " + be.ParentManga.MangaName;

                                            slide.Image = bitmp;

                                            slide.FontSize = 25;
                                            slide.Foreground = Brushes.Red;
                                            slide.FontStyle = FontStyles.Oblique;
                                            metroBanner.Slides.Add(slide);
                                        }));
                                }

                                Dispatcher.BeginInvoke(new EmptyDelegate(() =>
                                    {
                                        metroBanner.Slide = metroBanner.Slides[0];
                                        metroBanner.Start();


                                        LatestReleaseGB.NotificationsCount = metroBanner.Slides.Count;
                                    }));
                            }
                        });

                        Dispatcher.BeginInvoke(
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
                        t1.Wait();
                        t2.Wait();
                        t1.Dispose();
                        t2.Dispose();
                    });

            System.Timers.Timer t = new System.Timers.Timer();
            System.Timers.ElapsedEventHandler h = null;
            h = new System.Timers.ElapsedEventHandler((a, b) =>
                 {
                     mainTsk.Wait();
                     mainTsk.Dispose();
                     t.Elapsed -= h;
                     t.Stop();
                     t.Dispose();

                     GC.KeepAlive(metroBanner);
                     GC.Collect();
                 }); ;
            t.Elapsed += h;
            t.Interval = 10000;
            t.Start();


            Application.Current.Exit += new ExitEventHandler((o, er) => { Global.Current_Exit(o, er); });
            this.Closing += new System.ComponentModel.CancelEventHandler((s, er) =>
            {
                Global.SaveQueue(this.DlQueue);

                metroBanner.Stop();
            });
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
                                    Dispatcher.BeginInvoke(
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
                                tsk.Dispose();
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

        #region Queue Downloading
        public bool isQueueRunning = false;
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

                    if (isQueueRunning == false)
                        QueueStatuslbl.Content = string.Format("There are currently {0} items in your queue.", DlQueue.Count);
                }
            }
        }
        internal void CallStartQueueProcess()
        {
            QueueRunning = true;
            var queueRunner = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                int i = 0;
                while (DlQueue.Count > 0)
                {
                    isQueueRunning = true;

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

                        Global.DownloadMangaBook(q.Manga, q.Book, (dlBook) =>
                        {
                            q.Downloading = false; q.Status = QueueStatus.Completed; DlQueue.Remove(q); Dispatcher.Invoke(new EmptyDelegate(() =>
                            {
                                DlQueueList.ItemsSource = DlQueue;
                                DlQueueList.Items.Refresh();
                                QueueStatuslbl.Content = string.Format("Done! {0} items left in queue.", DlQueue.Count);
                                Global.DisplayNotification(string.Format("{0} has downloaded.", q.Name), "Download Complete");
                                CurrProg.Value = 0;
                                Count.Content = string.Format("{0}%", 0);

                                Global.CollectionBooks.Add(dlBook);
                                Global.BookCollection = Global.CollectionBooks;

                                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                                {
                                    //if Win7
                                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                                    this.TaskbarItemInfo.ProgressValue = 0;
                                }
                            }));
                        }, (curr, total) =>
                        {
                            Dispatcher.Invoke(new UpdateDelegate((cur, tot) =>
                            {
                                int percent = ((((cur < tot) ? cur + 1 : cur) * 100) / tot);
                                CurrProg.Value = percent;
                                Count.Content = string.Format("{0}%", percent);

                                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                                {
                                    //if Win7
                                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                                    this.TaskbarItemInfo.ProgressValue = (double)percent / 100;
                                }
                            }), curr, total);
                        });
                        while (q.Downloading)
                            System.Threading.Thread.Sleep(30000);
                    }
                }
                QueueRunning = false;
                Global.DisplayNotification("Your queue has been emptied!", "Empty Queue!");
            });
        }
        #endregion

        private void searchTile_Click(object sender, RoutedEventArgs e)
        {
            var sw = new SearchWindow();
            sw.Show();
        }
        internal void InvokeReadManga(Manga m)
        {
            MetroTabItem mti = new MetroTabItem();
            mti.IsClosable = true;
            mti.Header = m.MangaName;
            mti.Content = new MangaInfoControl(m);

            metroTabControl1.Items.Add(mti);

            metroTabControl1.SelectedItem = mti;
        }
        private void searchTile_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        
    }
}
