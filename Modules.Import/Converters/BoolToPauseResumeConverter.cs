using System;
using System.Globalization;
using System.Windows.Data;

namespace Modules.Import.Converters
{
    public class BoolToPauseResumeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Resume scan";

            return "Pause scan";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
