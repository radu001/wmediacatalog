using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Modules.Albums.Converters
{
    public class VerticalAlignmentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return VerticalAlignment.Center;

            return VerticalAlignment.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
