using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Dialogs.Converters
{
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sval = value as string;

            if (String.IsNullOrWhiteSpace(sval))
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
