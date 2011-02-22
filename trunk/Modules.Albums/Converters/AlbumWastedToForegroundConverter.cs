using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Modules.Albums.Converters
{
    public class AlbumWastedToForegroundConverter : IValueConverter
    {
        private static Brush NormalBrush = new SolidColorBrush(Colors.Black);
        private static Brush WasteBrush = new SolidColorBrush(Colors.LightSlateGray);

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return WasteBrush;

            return NormalBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
