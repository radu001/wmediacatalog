using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BusinessObjects;

namespace Modules.Artists.Converters
{
    public class ArtistWastedToVisibilityConvreter : IValueConverter
    {
        public bool Inverse { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Artist artist = value as Artist;

            if (artist == null)
                return Visibility.Collapsed;

            if (!Inverse)
            {
                if (artist.IsWaste)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
            else
            {
                if (artist.IsWaste)
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
