using System;
using System.Globalization;
using System.Windows.Data;

namespace TagCloudLib
{
    public class TagTextConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tag = (ITag)value;
            return String.Format("{0} ({1})", tag.Name, tag.Rank);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
