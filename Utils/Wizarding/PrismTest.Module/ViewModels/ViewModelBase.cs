
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Prism.Wizards.Events;
namespace PrismTest.Module.ViewModels
{
    public class ViewModelBase
    {
        public ViewModelBase(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            CompleteStepCommand = new DelegateCommand<object>(OnCompleteStepCommand);
        }

        #region IInitialViewModel Members

        public DelegateCommand<object> CompleteStepCommand { get; private set; }

        private void OnCompleteStepCommand(object parameter)
        {
            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        #endregion

        private IEventAggregator eventAggregator;
    }
}
