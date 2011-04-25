using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Common.Controllers
{
    public class ControllerBase
    {
        public ControllerBase(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
        }

        protected readonly IUnityContainer container;
        protected readonly IEventAggregator eventAggregator;
        protected readonly IRegionManager regionManager;
    }
}
