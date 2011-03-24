
using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Common.Enums;
namespace Common.Dialogs.Converters
{
    public class NotificationImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NotificationType ntype = (NotificationType)value;

            switch (ntype)
            {
                case NotificationType.Error:
                    return errorImage;
                case NotificationType.Help:
                    return helpImage;
                case NotificationType.Info:
                    return infoImage;
                case NotificationType.Warning:
                    return warningImage;
                case NotificationType.Success:
                    return successImage;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private static BitmapImage GetImage(NotificationType imageType)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();

            switch (imageType)
            {
                case NotificationType.Error:
                    image.UriSource = new Uri("pack://application:,,,/Common.Content;component/Images/109_AllAnnotations_Error_48x48_72.png");
                    break;
                case NotificationType.Help:
                    image.UriSource = new Uri("pack://application:,,,/Common.Content;component/Images/109_AllAnnotations_Help_48x48_72.png");
                    break;
                case NotificationType.Info:
                    image.UriSource = new Uri("pack://application:,,,/Common.Content;component/Images/109_AllAnnotations_Info_48x48_72.png");
                    break;
                case NotificationType.Warning:
                    image.UriSource = new Uri("pack://application:,,,/Common.Content;component/Images/109_AllAnnotations_Warning_48x48_72.png");
                    break;
                case NotificationType.Success:
                    image.UriSource = new Uri("pack://application:,,,/Common.Content;component/Images/109_AllAnnotations_Default_48x48_72.png");
                    break;
            }
            image.EndInit();

            return image;
        }

        private static BitmapImage infoImage = GetImage(NotificationType.Info);
        private static BitmapImage warningImage = GetImage(NotificationType.Warning);
        private static BitmapImage helpImage = GetImage(NotificationType.Help);
        private static BitmapImage errorImage = GetImage(NotificationType.Error);
        private static BitmapImage successImage = GetImage(NotificationType.Success);
    }
}
