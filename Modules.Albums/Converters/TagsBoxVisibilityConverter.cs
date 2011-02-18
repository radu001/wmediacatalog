using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Modules.Albums.Enums;

namespace Modules.Albums.Converters
{
    public class TagsBoxVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            AlbumTabsEnum tabType = AlbumTabsEnum.General;

            if (Enum.TryParse<AlbumTabsEnum>(value.ToString(), out tabType))
            {
                if (tabType == AlbumTabsEnum.General)
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
