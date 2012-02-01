// -----------------------------------------------------------------------
// <copyright file="IntCalcConverter.cs" company="">
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
    public class IntCalcConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var equa = parameter as string;
            var bits = equa.Split(';');
            var result = 0;

            switch(bits[0])
            {
                case "*": return (int)value * int.Parse(bits[1]);
                case "+": return (int)value + int.Parse(bits[1]);
                case "-": return (int)value - int.Parse(bits[1]);
                case "/": return (int)value / int.Parse(bits[1]);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
