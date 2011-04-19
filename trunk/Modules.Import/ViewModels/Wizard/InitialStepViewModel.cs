using Microsoft.Practices.Prism.Events;
using Modules.Import.ViewModels.Wizard.Common;

namespace Modules.Import.ViewModels.Wizard
{
    public class InitialStepViewModel : WizardViewModelBase, IInitialStepViewModel
    {
        public InitialStepViewModel(IEventAggregator eventAggregator)
            :base(eventAggregator)
        {
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
    }
}
