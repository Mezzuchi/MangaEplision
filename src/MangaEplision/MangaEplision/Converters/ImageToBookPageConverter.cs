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
    public class ImageByteToBookPageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<WPFMitsuControls.BookPage> pages = new List<WPFMitsuControls.BookPage>();

            var coll = (System.Collections.ObjectModel.Collection<byte[]>)value;

            foreach (var by in coll)
            {
                using (var ms = new MemoryStream(by))
                {

                    var img = new Image();
                    img.Source = BitmapFrame.Create(ms);
                    WPFMitsuControls.BookPage bp = new WPFMitsuControls.BookPage();
                    bp.Content = img;
                    pages.Add(bp);
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
