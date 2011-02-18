using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Dialogs.Converters
{
    public class RatingConverter : IValueConverter
    {
        const double Max = 1;
        const double Min = 0;
        const int ItemsCount = 7;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ival = (int)value;

            return (double)ival/ItemsCount;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dval = (double)value;

            return (int)(dval * ItemsCount);
        }

        #endregion
    }
}
