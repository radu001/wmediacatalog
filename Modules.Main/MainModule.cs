
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Main.Controllers;
using Modules.Main.Services;
using Modules.Main.ViewModels;
namespace Modules.Main
{
    public class MainModule : IModule
    {
        public MainModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IMainViewModel, MainViewModel>();

            mainController = container.Resolve<MainController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private MainController mainController;
    }
}
