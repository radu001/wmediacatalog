
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
            var settings = new WizardSettings();
            settings.AddStep<IInitialViewModel, InitialViewModel, InitialView>(0, "Initial step");
            settings.AddStep<IStep1ViewModel, Step1ViewModel, Step1View>(1, "Step1");
            settings.AddStep<IStep2ViewModel, Step2ViewModel, Step2View>(2, "Step2");
            settings.AddStep<ICompleteViewModel, CompleteViewModel, CompleteView>(3, "Complete wizard");
            Wizard w = new Wizard(container, settings, "MainRegion", "wizard1");
        }

        #endregion

        private IUnityContainer container;
        private IRegionManager regionManager;
    }
}
