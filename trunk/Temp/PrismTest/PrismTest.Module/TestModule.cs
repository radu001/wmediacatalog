
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.Wizards;
using PrismTest.Module.ViewModels;
using PrismTest.Module.Views;
namespace PrismTest.Module
{
    public class TestModule : IModule
    {
        public TestModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            IWizardContext context = new WizardContext();
            context.AddStep<IInitialViewModel, InitialViewModel, InitialView>(0, "Initial step");
            context.AddStep<IStep1ViewModel, Step1ViewModel, Step1View>(1, "Step1");
            Wizard w = new Wizard(container, context, "MainRegion", "wizard1");
        }

        #endregion

        private IUnityContainer container;
        private IRegionManager regionManager;
    }
}
