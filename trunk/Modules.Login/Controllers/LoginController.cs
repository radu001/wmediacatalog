
using Common;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Login.Views;
namespace Modules.Login.Controllers
{
    public class LoginController
    {
        public LoginController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            InitViews();
            DisplayView(ViewNames.LoginView);
        }

        private void InitViews()
        {
            IRegion mainRegion = GetMainRegion();
            mainRegion.Add(container.Resolve<LoginView>(), ViewNames.LoginView);
        }

        private void DisplayView(string viewName)
        {
            IRegion mainRegion = GetMainRegion();
            object view = mainRegion.GetView(viewName);

            if (view != null)
                mainRegion.Activate(view);
        }

        private IRegion GetMainRegion()
        {
            return regionManager.Regions[RegionNames.MainRegion];
        }

        private IUnityContainer container;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;
    }
}
