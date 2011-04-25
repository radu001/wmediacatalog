using System;
using System.Globalization;
using System.Windows.Data;

namespace Modules.Import.Converters
{
    public class ProgressPercentageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float dval = (float)value;
            return String.Format("{0:0.00}", dval) + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
