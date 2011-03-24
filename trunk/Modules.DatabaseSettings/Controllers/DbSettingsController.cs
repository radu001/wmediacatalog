
using Common;
using Common.Controllers;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.DatabaseSettings.Views;
namespace Modules.DatabaseSettings.Controllers
{
    public class DbSettingsController : WorkspaceControllerBase
    {
        public DbSettingsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator)
        {
            eventAggregator.GetEvent<SetupDatabaseEvent>().Subscribe(OnSetupDatabaseEvent, true);
            eventAggregator.GetEvent<EndSetupDatabaseEvent>().Subscribe(OnEndSetupDatabaseEvent, true);
        }

        private void OnSetupDatabaseEvent(object parameter)
        {
            IRegion mainRegion = GetMainRegion();
            var view = mainRegion.GetView(ViewNames.ConnectionSettingsView);
            if (view != null)
            {
                mainRegion.Activate(view);
            }
        }

        private void OnEndSetupDatabaseEvent(object parameter)
        {
            IRegion mainRegion = GetMainRegion();
            var view = mainRegion.GetView(ViewNames.LoginView);
            if (view != null)
            {
                mainRegion.Activate(view);
            }
        }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<DatabaseToolsView>(), WorkspaceNameEnum.DatabaseTools.ToString());

            IRegion mainRegion = GetMainRegion();
            mainRegion.Add(container.Resolve<ConnectionSettingsView>(), ViewNames.ConnectionSettingsView);
        }

        private IRegion GetMainRegion()
        {
            return regionManager.Regions[RegionNames.MainRegion];
        }
    }
}
