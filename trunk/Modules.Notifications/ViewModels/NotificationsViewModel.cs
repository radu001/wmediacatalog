
using Common.Enums;
using Common.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
namespace Modules.Notifications.ViewModels
{
    public class NotificationsViewModel : DialogViewModelBase, INotificationsViewModel
    {
        public NotificationsViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
        }

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

        public override void OnSuccessCommand(object parameter)
        {
            DialogResult = true;
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        #region Private fields

        private NotificationType notificationType;
        private string messageText;

        #endregion
    }
}
