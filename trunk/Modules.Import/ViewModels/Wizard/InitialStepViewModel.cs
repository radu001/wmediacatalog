using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.ViewModels.Wizard.Common;
using Prism.Wizards.Events;

namespace Modules.Import.ViewModels.Wizard
{
    public class InitialStepViewModel : WizardViewModelBase, IInitialStepViewModel
    {
        public InitialStepViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        #region IInitialStepViewModel Members

        public string Message
        {
            get
            {
                return @"Welcome to import wizard. It will guide you through the process of " +
                         "importing data from your local storage(s) into media library";
            }
        }

        #endregion

        public override void OnContinueCommand(object parameter)
        {
            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        private IEventAggregator eventAggregator;
    }
}
