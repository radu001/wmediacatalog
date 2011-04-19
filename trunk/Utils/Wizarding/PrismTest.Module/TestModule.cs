
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrismTest.Module.Controllers;
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
            container.RegisterType<IModuleViewModel, ModuleViewModel>();

            regionManager.RegisterViewWithRegion("MainRegion", typeof(ModuleView));
            controller = container.Resolve<ModuleController>();


        }

        #endregion

        private IUnityContainer container;
        private IRegionManager regionManager;
        private ModuleController controller;
    }
}
