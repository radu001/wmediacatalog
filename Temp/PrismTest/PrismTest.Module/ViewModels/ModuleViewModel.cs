
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using PrismTest.Module.Events;
namespace PrismTest.Module.ViewModels
{
    public class ModuleViewModel : IModuleViewModel
    {
        public ModuleViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            StartWizardCommand = new DelegateCommand<object>(OnStartWizardCommand);
        }

        #region IModuleViewModel Members

        public DelegateCommand<object> StartWizardCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnStartWizardCommand(object parameter)
        {
            eventAggregator.GetEvent<StartWizardEvent>().Publish(null);
        }

        #endregion

        private IEventAggregator eventAggregator;
    }
}
