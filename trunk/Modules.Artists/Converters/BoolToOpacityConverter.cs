using System;
using System.Globalization;
using System.Windows.Data;

namespace Modules.Artists.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return 0.3d;

            return 1d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
