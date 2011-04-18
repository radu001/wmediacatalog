
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prism.Wizards.Data;
using Prism.Wizards.Events;
using Prism.Wizards.Utils;
namespace Prism.Wizards.ViewModels
{
    internal class WizardViewModel : NotificationObject, IWizardViewModel
    {
        public WizardViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;

            NextStepCommand = new DelegateCommand<object>(OnNextStepCommand);
            PrevStepCommand = new DelegateCommand<object>(OnPrevStepCommand);
            MoveToStepCommand = new DelegateCommand<object>(OnMoveToStepCommand);

            eventAggregator.GetEvent<UpdateNavBarEvent>().Subscribe(OnUpdateNavBarEvent, true);

            UpdateSteps();
        }

        public void UnsubscribeEvents()
        {
            eventAggregator.GetEvent<UpdateNavBarEvent>().Unsubscribe(OnUpdateNavBarEvent);
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

        public IEnumerable<WizardStep> Steps
        {
            get
            {
                return steps;
            }
            private set
            {
                steps = value;
                RaisePropertyChanged(() => Steps);
            }
        }

        public DelegateCommand<object> NextStepCommand { get; private set; }

        public DelegateCommand<object> PrevStepCommand { get; private set; }

        public DelegateCommand<object> MoveToStepCommand { get; private set; }

        #endregion

        #region Private methods

        private void UpdateSteps()
        {
            var context = container.Resolve<IWizardContext>();
            Steps = context.ToArray();
        }

        private void OnNextStepCommand(object parameter)
        {
            RaiseWizardNavigationEvent(true, null);
        }

        private void OnPrevStepCommand(object parameter)
        {
            RaiseWizardNavigationEvent(false, null);
        }

        private void OnMoveToStepCommand(object parameter)
        {
            var step = parameter as WizardStep;
            if (step != null)
            {
                RaiseWizardNavigationEvent(false, step);
            }
        }

        private void RaiseWizardNavigationEvent(bool moveForward, WizardStep step)
        {
            eventAggregator.GetEvent<WizardNavigationEvent>().Publish(new NavigationSettings()
            {
                WizardName = WizardName,
                MoveForward = moveForward,
                Step = step
            });
        }

        private void OnUpdateNavBarEvent(IWizardContext context)
        {
            UpdateSteps();
        }

        #endregion

        #region Private fields

        private IUnityContainer container;
        private IEventAggregator eventAggregator;

        private string wizardName;
        private string wizardRegionName;
        private IEnumerable<WizardStep> steps;

        #endregion

    }
}
