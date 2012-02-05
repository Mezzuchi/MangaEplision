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
            this.SizeChanged += new SizeChangedEventHandler(MangaReaderWin_SizeChanged);
        }

        void MangaReaderWin_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateBookDisplay();
        }
        void updateBookDisplay()
        {
            BookControl.GetBindingExpression(WPFMitsuControls.Book.ItemsSourceProperty).UpdateTarget(); //Causes flicking but it'll do.
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
        }

        private void previousBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookControl.AnimateToPreviousPage(true, 700);
            }
            catch (Exception)
            {

            }
            updateBookDisplay();
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            //BookControl.CurrentPage = WPFMitsuControls.BookCurrentPage.LeftSheet;

            try
            {
                BookControl.AnimateToNextPage(true, 700);
            }
            catch (Exception)
            {

            }
            updateBookDisplay();
            this.InvalidateMeasure();
        }
    }
}
