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
using MangaEplision.Converters;
using MangaEplision.Base;
using WPFMitsuControls;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for MangaReaderWin.xaml
    /// </summary>
    public partial class MangaReaderWin : Window
    {
        public MangaReaderWin()
        {
            InitializeComponent();
            this.Loaded += MangaReaderWin_Loaded;
            this.SizeChanged += MangaReaderWin_SizeChanged;
            this.KeyDown += MangaReaderWin_KeyDown;
            this.Unloaded += MangaReaderWin_Unloaded;
        }

        void MangaReaderWin_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MangaReaderWin_Loaded;
            this.SizeChanged -= MangaReaderWin_SizeChanged;
            this.KeyDown -= MangaReaderWin_KeyDown;
            this.Unloaded -= MangaReaderWin_Unloaded;

            BindingOperations.ClearAllBindings(this);
            BindingOperations.ClearAllBindings(BookControl);
            BindingOperations.ClearAllBindings(Ctrlgrid);
            BindingOperations.ClearAllBindings(slider1);
            BindingOperations.ClearAllBindings(checkBox1);
        }

        void MangaReaderWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                try
                {
                    BookControl.AnimateToPreviousPage(true, 700);
                }
                catch (Exception)
                {
                }
                return;
            }

            if (e.Key == Key.Right)
            {
                try
                {
                    BookControl.AnimateToNextPage(true, 700);
                }
                catch (Exception)
                {
                }
                return;
            }
        }

        void MangaReaderWin_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateBookDisplay();
        }
        void updateBookDisplay()
        {
            BookControl.GetBindingExpression(WPFMitsuControls.Book.ItemsSourceProperty).UpdateTarget(); //Causes flicking but it'll do.
            slider1.Value = BookControl.CurrentSheetIndex;
            if (BookControl.ItemsSource == null)
            {
                slider1.Maximum = 0;
            }
            else
            {
                var max = ((List<BookPage>)(BookControl.ItemsSource)).Count;
                slider1.Maximum = max - 1;
            }
            slider1.Minimum = 0;
        }
        void MangaReaderWin_Loaded(object sender, RoutedEventArgs e)
        {
            /*  var bk = (MangaEplision.Base.Book)this.DataContext;
              foreach (var x in (List<WPFMitsuControls.BookPage>)(new ImageUriToBookPageConverter().Convert(
                  (bk.PageLocalUrls),
                  null,
                  null,
                  null)))
              {
                  BookControl.Items.Add(x);
              }

              /*using (var ms = new System.IO.MemoryStream(bk.Pages[0]))
              {
                  var img = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                  TestImage.Source = img;
                  this.Background = new ImageBrush(img);
                  var p = 0;
              }*/
            this.MinHeight = this.ActualHeight;
            this.MinWidth = this.ActualWidth;

            slider1.Minimum = -1;
        }

        private void previousBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookControl.AnimateToPreviousPage(true, 700);
                updateBookDisplay();
            }
            catch (Exception)
            {

            }

        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            //BookControl.CurrentPage = WPFMitsuControls.BookCurrentPage.LeftSheet;

            try
            {
                BookControl.AnimateToNextPage(true, 700);
                updateBookDisplay();
                this.InvalidateMeasure();
            }
            catch (Exception)
            {

            }

        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateBookDisplay();
        }
        ~MangaReaderWin()
        {
            this.DataContext = null;
            //GC.Collect();
        }
    }
}
