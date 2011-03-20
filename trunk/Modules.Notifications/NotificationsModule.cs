using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Notifications.Controllers;
using Modules.Notifications.ViewModels;

namespace Modules.Notifications
{
    public class NotificationsModule : IModule
    {
        public NotificationsModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }


        public void Initialize()
        {
            container.RegisterType<INotificationsViewModel, NotificationsViewModel>();

            controller = container.Resolve<NotificationsController>();
        }

        private IRegionManager regionManager;
        private IUnityContainer container;
        private NotificationsController controller;
    }
}
