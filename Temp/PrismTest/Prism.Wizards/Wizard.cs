using System.Linq;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
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
            RegionName = wizardRegionName;
            Name = wizardName;

            RegisterEvents();
            RegisterViewModel(wizardContext);
        }

        #region Event handlers

        private void RegisterEvents()
        {
            var eventAggregator = wizardContainer.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<WizardNavigationEvent>().Subscribe(OnWizardNavigationEvent);
        }

        private void OnWizardNavigationEvent(NavigationSettings settings)
        {
            //TODO
        }

        #endregion

        private void RegisterViewModel(IWizardContext wizardContext)
        {
            wizardContainer.RegisterInstance<IWizardContext>(wizardContext);
            wizardContainer.RegisterType<IWizardViewModel, WizardViewModel>(
                new InjectionProperty("WizardName", Name),
                new InjectionProperty("WizardRegionName", RegionName));

            var regionManager = wizardContainer.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName, () =>
            {
                return wizardContainer.Resolve<WizardView>();
            });

            RegisterViews(wizardContext);
        }

        private void RegisterViews(IWizardContext context)
        {
            var regionManager = wizardContainer.Resolve<IRegionManager>();
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
        }

        private IRegion GetStepsRegion(IRegionManager regionManager)
        {
            string stepsRegionName = StepsRegionNameResolver.ResolveRegionName(Name, RegionName);
            return regionManager.Regions[stepsRegionName];
        }

        private IUnityContainer wizardContainer;
    }
}
