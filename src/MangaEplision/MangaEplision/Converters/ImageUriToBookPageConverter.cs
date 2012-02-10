// -----------------------------------------------------------------------
// <copyright file="ImageToBookPageConverter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MangaEplision.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
    using System.Windows.Controls;
    using System.IO;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ImageUriToBookPageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<WPFMitsuControls.BookPage> pages = new List<WPFMitsuControls.BookPage>();

            var coll = (System.Collections.ObjectModel.Collection<string>)value;

            foreach (var by in coll)
            {
                try
                {
                    var img = new Image();

                    var bitmp = new BitmapImage();
                    bitmp.BeginInit();
                    bitmp.UriSource = new Uri(by);
                    bitmp.CacheOption = BitmapCacheOption.None;
                    bitmp.EndInit();

                    bitmp.Freeze();
                    img.Source = bitmp;

                    WPFMitsuControls.BookPage bp = new WPFMitsuControls.BookPage();
                    bp.Content = new Grid();
                    ((Grid)bp.Content).Children.Add(img);
                    pages.Add(bp);
                }
                catch (Exception)
                {

                }
            }

            return pages;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
