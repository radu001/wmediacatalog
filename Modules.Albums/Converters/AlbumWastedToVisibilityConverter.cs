using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BusinessObjects;

namespace Modules.Albums.Converters
{
    public class AlbumWastedToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Album album = value as Album;

            if (album == null)
                return Visibility.Collapsed;

            if (!Inverse)
            {
                if (album.IsWaste)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
            else
            {
                if (album.IsWaste)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
