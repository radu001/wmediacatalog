
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Prism.Wizards;
using PrismTest.Module.Views;
namespace PrismTest.Module.ViewModels
{
    public class ModuleViewModel : IModuleViewModel
    {
        public ModuleViewModel(IEventAggregator eventAggregator, IUnityContainer container)
        {
            this.eventAggregator = eventAggregator;
            this.container = container;

            StartWizardCommand = new DelegateCommand<object>(OnStartWizardCommand);
        }

        #region IModuleViewModel Members

        public DelegateCommand<object> StartWizardCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnStartWizardCommand(object parameter)
        {
            var settings = new WizardSettings();
            settings.EnableNavBar = false;
            settings.AddStep<IInitialViewModel, InitialViewModel, InitialView>(0, "Initial step");
            settings.AddStep<IStep1ViewModel, Step1ViewModel, Step1View>(1, "Step1");
            settings.AddStep<IStep2ViewModel, Step2ViewModel, Step2View>(2, "Step2");
            settings.AddStep<ICompleteViewModel, CompleteViewModel, CompleteView>(3, "Complete wizard");
            Wizard w = new Wizard(container, settings, "wizard1");
            w.Start();

            w = null;
        }

        #endregion

        private IEventAggregator eventAggregator;
        private IUnityContainer container;
    }
}
