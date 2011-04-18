using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.Wizards;
using PrismTest.Module.Events;
using PrismTest.Module.ViewModels;
using PrismTest.Module.Views;

namespace PrismTest.Module.Controllers
{
    public class ModuleController
    {
        public ModuleController(IEventAggregator eventAggregator, IUnityContainer container, IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.container = container;

            eventAggregator.GetEvent<StartWizardEvent>().Subscribe(OnStartWizardEvent, true);
        }

        private void OnStartWizardEvent(object parameter)
        {
            var settings = new WizardSettings();
            settings.AddStep<IInitialViewModel, InitialViewModel, InitialView>(0, "Initial step");
            settings.AddStep<IStep1ViewModel, Step1ViewModel, Step1View>(1, "Step1");
            settings.AddStep<IStep2ViewModel, Step2ViewModel, Step2View>(2, "Step2");
            settings.AddStep<ICompleteViewModel, CompleteViewModel, CompleteView>(3, "Complete wizard");
            Wizard w = new Wizard(container, settings, "wizard1");
            w.Start();
        }

        private IEventAggregator eventAggregator;
        private IUnityContainer container;
    }
}
