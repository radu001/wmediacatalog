using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Prism.Wizards.Events;

namespace Modules.Import.ViewModels.Wizard
{
    public class CompleteViewModel : ViewModelBase, ICompleteViewModel
    {
        public CompleteViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            CompleteWizardCommand = new DelegateCommand<object>(OnCompleteWizardCommand);
        }

        #region ICompleteViewModel Members

        public DelegateCommand<object> CompleteWizardCommand { get; set; }

        #endregion

        #region Private methods

        private void OnCompleteWizardCommand(object parameter)
        {
            eventAggregator.GetEvent<CompleteWizardEvent>().Publish(null);
        }

        #endregion
    }
}
