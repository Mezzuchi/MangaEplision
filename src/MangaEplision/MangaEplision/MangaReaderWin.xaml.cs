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
        }

        void MangaReaderWin_Loaded(object sender, RoutedEventArgs e)
        {
            var bk = (MangaEplision.Base.Book)this.DataContext;
            /*foreach (var x in (List<WPFMitsuControls.BookPage>)(new ImageByteToBookPageConverter().Convert(
                (bk.Pages),
                null,
                null,
                null)))
            {
                BookControl.Items.Add(x);
            }*/

            using (var ms = new System.IO.MemoryStream(bk.Pages[0]))
            {
                var img = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                TestImage.Source = img;
            }
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
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            BookControl.CurrentPage = WPFMitsuControls.BookCurrentPage.RightSheet;

            try
            {
                BookControl.AnimateToNextPage(true, 700);
            }
            catch (Exception)
            {

            }
        }
    }
}
