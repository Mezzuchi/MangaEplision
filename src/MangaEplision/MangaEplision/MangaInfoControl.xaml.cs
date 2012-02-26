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
            this.Loaded += new RoutedEventHandler(MangaInfoControl_Loaded);
            this.Unloaded += new RoutedEventHandler(MangaInfoControl_Unloaded);
        }

        void MangaInfoControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(MangaInfoControl_Loaded);
            this.Unloaded -= new RoutedEventHandler(MangaInfoControl_Unloaded);
        }

        void MangaInfoControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void downloadTile_Click(object sender, RoutedEventArgs e)
        {
            if (BooksLsView.SelectedItem == null)
                MessageBox.Show("Please select a book before pressing this button!");
            else
            {
                if (BooksLsView.SelectedItems.Count > 1)
                {
                    for (int i = 0; i < BooksLsView.SelectedItems.Count; i++)
                    {
                        if (!Global.GetBookExist((MangaEplision.Base.Manga)this.DataContext, (BookEntry)BooksLsView.SelectedItems[i]))
                        {
                            ((MainWindow)Application.Current.MainWindow).AddToQueue((BookEntry)BooksLsView.SelectedItems[i], (MangaEplision.Base.Manga)this.DataContext);
                        }
                    }
                    if (!((MainWindow)Application.Current.MainWindow).QueueRunning)
                        ((MainWindow)Application.Current.MainWindow).CallStartQueueProcess();
                }
                else
                {
                    if (!Global.GetBookExist((MangaEplision.Base.Manga)this.DataContext, (BookEntry)BooksLsView.SelectedItem))
                    {
                        ((MainWindow)Application.Current.MainWindow).AddToQueue((BookEntry)BooksLsView.SelectedItem, (MangaEplision.Base.Manga)this.DataContext);
                        if (!((MainWindow)Application.Current.MainWindow).QueueRunning)
                            ((MainWindow)Application.Current.MainWindow).CallStartQueueProcess();
                    }
                    else
                    {
                        MessageBox.Show("You already have this book!");
                    }
                }
                ((MainWindow)Application.Current.MainWindow).metroTabControl1.SelectedIndex = 2;
            }
        }

        private void BooksLsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Global.GetBookExist((MangaEplision.Base.Manga)this.DataContext, (BookEntry)BooksLsView.SelectedItem))
            {
                ((MainWindow)Application.Current.MainWindow).AddToQueue((BookEntry)BooksLsView.SelectedItem, (MangaEplision.Base.Manga)this.DataContext);
                if (!((MainWindow)Application.Current.MainWindow).QueueRunning)
                    ((MainWindow)Application.Current.MainWindow).CallStartQueueProcess();
            }
            else
            {
                MessageBox.Show("You already have this book!");
            }
        }
    }
}
