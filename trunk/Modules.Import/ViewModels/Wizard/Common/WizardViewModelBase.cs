using Common.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Prism.Wizards.Events;

namespace Modules.Import.ViewModels.Wizard.Common
{
    public class WizardViewModelBase : NotificationObject
    {
        public WizardViewModelBase(IEventAggregator eventAggregator)
        {
            ContinueCommand = new DelegateCommand<object>((o) =>
            {
                eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
            });
        }

        public DelegateCommand<object> ContinueCommand { get; private set; }
    }
}
