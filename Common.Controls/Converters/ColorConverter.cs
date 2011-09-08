using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Common.Controls.Converters
{
    public class ColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorStr = value as string;
            if (colorStr == null)
                return Colors.Transparent;

            return System.Windows.Media.ColorConverter.ConvertFromString(colorStr);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Transparent.ToString();

            return value.ToString();
        }

        #endregion
    }
}
