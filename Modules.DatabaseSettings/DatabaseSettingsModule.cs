using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.DatabaseSettings.Controllers;
using Modules.DatabaseSettings.Services;
using Modules.DatabaseSettings.ViewModels;

namespace Modules.DatabaseSettings
{
    public class DatabaseSettingsModule : IModule
    {
        public DatabaseSettingsModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();

            container.RegisterType<IConnectionSettingsViewModel, ConnectionSettingsViewModel>();
            container.RegisterType<IDatabaseToolsViewModel, DatabaseToolsViewModel>();

            controller = container.Resolve<DbSettingsController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private DbSettingsController controller;
    }
}
