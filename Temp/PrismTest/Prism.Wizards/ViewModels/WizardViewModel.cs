
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prism.Wizards.Events;
using Prism.Wizards.Utils;
namespace Prism.Wizards.ViewModels
{
    public class WizardViewModel : NotificationObject, IWizardViewModel
    {
        public WizardViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            Context = container.Resolve<IWizardContext>();

            NextStepCommand = new DelegateCommand<object>(OnNextStepCommand);
            PrevStepCommand = new DelegateCommand<object>(OnPrevStepCommand);
        }

        #region IWizardViewModel Members

        public string WizardName
        {
            get
            {
                return wizardName;
            }
            set
            {
                wizardName = value;
            }
        }

        public string WizardRegionName
        {
            get
            {
                return wizardRegionName;
            }
            set
            {
                wizardRegionName = value;
            }
        }

        public string StepRegionName
        {
            get
            {
                return StepsRegionNameResolver.ResolveRegionName(WizardName, WizardRegionName);
            }
        }

        public IWizardContext Context
        {
            get
            {
                return context;
            }
            private set
            {
                context = value;
            }
        }

        public DelegateCommand<object> NextStepCommand { get; private set; }

        public DelegateCommand<object> PrevStepCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnNextStepCommand(object parameter)
        {
            RaiseWizardNavigationEvent(true);
        }

        private void OnPrevStepCommand(object parameter)
        {
            RaiseWizardNavigationEvent(false);
        }

        private void RaiseWizardNavigationEvent(bool moveForward)
        {
            eventAggregator.GetEvent<WizardNavigationEvent>().Publish(new NavigationSettings()
            {
                WizardName = WizardName,
                MoveForward = moveForward
            });
        }

        #endregion

        #region Private fields

        private IUnityContainer container;
        private IEventAggregator eventAggregator;
        private IWizardContext context;

        private string wizardName;
        private string wizardRegionName;

        #endregion

    }
}
