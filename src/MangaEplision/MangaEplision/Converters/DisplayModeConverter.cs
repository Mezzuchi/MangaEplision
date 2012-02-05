// -----------------------------------------------------------------------
// <copyright file="DisplayModeConverter.cs" company="">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DisplayModeConverter :IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var m = (WPFMitsuControls.BookDisplayMode)value;
            if (m == WPFMitsuControls.BookDisplayMode.Normal)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch((bool)value)
            {
                case true: return WPFMitsuControls.BookDisplayMode.ZoomOnPage;
                case false: return WPFMitsuControls.BookDisplayMode.Normal;
            }
            return WPFMitsuControls.BookDisplayMode.Normal;
        }

        #endregion
    }
}
