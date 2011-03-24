
using Common.Dialogs;
using Common.Entities;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Notifications.ViewModels;
using Modules.Notifications.Views;
namespace Modules.Notifications.Controllers
{
    public class NotificationsController
    {
        public NotificationsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            eventAggregator.GetEvent<NotificationEvent>().Subscribe(OnNotificationReceived, true);
        }

        private void OnNotificationReceived(NotificationInfo info)
        {
            if (info == null)
                return;

            INotificationsViewModel viewModel = container.Resolve<INotificationsViewModel>();
            viewModel.MessageText = info.Text;
            viewModel.NotificationType = info.NotificationType;

            CommonDialog dialog = new CommonDialog()
            {
                HeaderText = info.NotificationType.ToString(),
                DialogButtons = DialogButtons.Ok,
                DialogContent = new NotificationControl(viewModel)
            };

            dialog.ShowDialog();
        }

        private readonly IUnityContainer container;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
    }
}
