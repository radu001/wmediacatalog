
using System.Windows.Data;
namespace Common.Dialogs.Converters
{
    public class HeaderTextConverter : IValueConverter
    {
        public string EntityName { get; set; }

        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "Edit " + EntityName;

            return "Create new " + EntityName;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
