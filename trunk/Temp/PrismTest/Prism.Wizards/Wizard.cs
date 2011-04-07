using System.Linq;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.Wizards.Data;
using Prism.Wizards.Events;
using Prism.Wizards.Utils;
using Prism.Wizards.ViewModels;
using Prism.Wizards.Views;

namespace Prism.Wizards
{
    public class Wizard
    {
        public string RegionName { get; private set; }

        public string Name { get; private set; }

        public Wizard(IUnityContainer container, IWizardContext wizardContext, string wizardRegionName, string wizardName)
        {
            wizardContainer = container.CreateChildContainer();
            regionManager = wizardContainer.Resolve<IRegionManager>();
            eventAggregator = wizardContainer.Resolve<IEventAggregator>();
            RegionName = wizardRegionName;
            Name = wizardName;

            RegisterEvents();
            RegisterViewModel(wizardContext);
        }

        #region Event handlers

        private void RegisterEvents()
        {
            eventAggregator.GetEvent<WizardNavigationEvent>().Subscribe(OnWizardNavigationEvent, true);
        }

        private void OnWizardNavigationEvent(NavigationSettings settings)
        {
            var context = wizardContainer.Resolve<IWizardContext>();

            if (settings.Step == null)
            {
                int currentStep = context.CurrentStep;

                if (settings.MoveForward && currentStep >= context.StepsCount)
                    return;

                if (!settings.MoveForward && currentStep <= 0)
                    return;

                if (settings.MoveForward)
                {
                    currentStep += 1;
                    context.CurrentStep = currentStep;
                }
                else
                {
                    currentStep -= 1;
                    context.CurrentStep = currentStep;
                }

                var step = context.Where(s => s.Index == currentStep).FirstOrDefault();
                if (step != null)
                {
                    SwitchStepView(step, context);
                }
            }
            else
            {
                SwitchStepView(settings.Step, context);
            }
        }

        private void SwitchStepView(WizardStep step, IWizardContext context)
        {
            var previousStep = context.Where(s => s.IsCurrent == true).FirstOrDefault();
            if (previousStep != null)
            {
                previousStep.IsCurrent = false;
                if (previousStep.Index == step.Index) // don't switch to already active step
                    return;
            }

            var region = GetStepsRegion(regionManager);
            var viewToActivate = region.Views.Where((v) => v.GetType() == step.View).FirstOrDefault();
            if (viewToActivate != null)
            {
                region.Activate(viewToActivate);
                eventAggregator.GetEvent<UpdateNavBarEvent>().Publish(context);

                step.IsCurrent = true;
            }
        }

        #endregion

        private void RegisterViewModel(IWizardContext wizardContext)
        {
            wizardContainer.RegisterInstance<IWizardContext>(wizardContext);
            wizardContainer.RegisterType<IWizardViewModel, WizardViewModel>(
                new InjectionProperty("WizardName", Name),
                new InjectionProperty("WizardRegionName", RegionName));

            regionManager.RegisterViewWithRegion(RegionName, () =>
            {
                return wizardContainer.Resolve<WizardView>();
            });

            RegisterViews(wizardContext);
        }

        private void RegisterViews(IWizardContext context)
        {
            var stepsRegion = GetStepsRegion(regionManager);

            var orderedSteps = context.OrderBy((s) => s.Index).ToList();
            foreach (var step in orderedSteps)
            {
                wizardContainer.RegisterType(step.IViewModel, step.ViewModel);
                regionManager.RegisterViewWithRegion(stepsRegion.Name, () =>
                {
                    var viewType = step.View;
                    return wizardContainer.Resolve(viewType);
                });
            }

            var firstStep = orderedSteps.FirstOrDefault();
            if (firstStep != null)
            {
                firstStep.IsCurrent = true;
            }
        }

        private IRegion GetStepsRegion(IRegionManager regionManager)
        {
            string stepsRegionName = StepsRegionNameResolver.ResolveRegionName(Name, RegionName);
            return regionManager.Regions[stepsRegionName];
        }

        private IUnityContainer wizardContainer;
        private IEventAggregator eventAggregator;
        private IRegionManager regionManager;
    }
}
