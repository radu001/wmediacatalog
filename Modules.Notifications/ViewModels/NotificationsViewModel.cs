
using Common.Enums;
namespace Modules.Notifications.ViewModels
{
    public class NotificationsViewModel : INotificationsViewModel
    {
        #region INotificationsViewModel Members

        public NotificationType NotificationType
        {
            get
            {
                return notificationType;
            }
            set
            {
                notificationType = value;
            }
        }

        public string HeaderText
        {
            get
            {
                return headerText;
            }
            set
            {
                headerText = value;
            }
        }

        public string MessageText
        {
            get
            {
                return messageText;
            }
            set
            {
                messageText = value;
            }
        }

        #endregion

        #region Private fields

        private NotificationType notificationType;
        private string headerText;
        private string messageText;

        #endregion
    }
}
