using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using BusinessObjects;

namespace Modules.Listenings.Converters
{
    public class AlbumConverter : IValueConverter
    {
        public Listening Listening { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Album album = value as Album;
            if (album != null)
                return album.Name;

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String str = value as string;
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Listening.Album;
            }
        }

        #endregion
    }
}
