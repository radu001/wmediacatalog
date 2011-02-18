using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Common.Entities.Pagination;

namespace Common.Controls.Converters
{
    public class DateFilterInputVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IField field = value as IField;
            if (field != null)
            {
                if (field.FieldType == FieldTypeEnum.DateInterval)
                    return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
