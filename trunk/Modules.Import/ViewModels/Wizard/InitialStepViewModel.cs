using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Prism.Wizards.Events;

namespace Modules.Import.ViewModels.Wizard
{
    public class InitialStepViewModel : IInitialStepViewModel
    {
        public InitialStepViewModel(IEventAggregator eventAggregator)
        {
            ContinueCommand = new DelegateCommand<object>((o) =>
            {
                eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
            });
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

        public DelegateCommand<object> ContinueCommand { get; private set; }

        #endregion
    }
}
