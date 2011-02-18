
using Common;
using Common.Entities;
using Common.Events;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Main.Views;
namespace Modules.Main.Controllers
{
    public class MainController
    {
        public MainController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            InitViews();

            eventAggregator.GetEvent<LoginSucceededEvent>().Subscribe(OnLoginSucceeded, true);
        }

        private void InitViews()
        {
            IRegion mainRegion = GetMainRegion();
            mainRegion.Add(container.Resolve<MainView>(), ViewNames.MainView);
        }

        private void OnLoginSucceeded(AuthorizationInfo info)
        {
            DisplayView(ViewNames.MainView);
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
