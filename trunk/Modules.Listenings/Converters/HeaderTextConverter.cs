using System;
using System.Globalization;
using System.Windows.Data;

namespace Modules.Listenings.Converters
{
    public class HeaderTextConverter : IValueConverter
    {
        public string EntityName { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Create " + EntityName;

            return "View " + EntityName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
