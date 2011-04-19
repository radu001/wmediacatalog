using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace PrismTest.Module.Controllers
{
    public class ModuleController
    {
        public ModuleController(IEventAggregator eventAggregator, IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            //eventAggregator.GetEvent<StartWizardEvent>().Subscribe(OnStartWizardEvent, true);
        }

        private void OnStartWizardEvent(object parameter)
        {

        }

        private IUnityContainer container;
    }
}
