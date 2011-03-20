
using System.Windows;
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
        }

        private void OnSetupDatabaseEvent(object parameter)
        {
            IRegion dbSettingsRegion = GetDbSettingsRegion();
            object view = dbSettingsRegion.GetView(ViewNames.DbSettingsView);

            if (view != null)
            {
                if (dbSettingsView.Visibility == Visibility.Collapsed || dbSettingsView.Visibility == Visibility.Hidden)
                    dbSettingsView.ShowDialog();
                dbSettingsRegion.Activate(view);
            }
        }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<DatabaseToolsView>(), WorkspaceNameEnum.DatabaseTools.ToString());

            dbSettingsView = new DbSettingsView();
            RegionManager.SetRegionManager(dbSettingsView, regionManager);
            IRegion dbSettingsRegion = GetDbSettingsRegion();
            dbSettingsRegion.Add(container.Resolve<ConnectionSettingsView>(), ViewNames.DbSettingsView);
        }

        private IRegion GetDbSettingsRegion()
        {
            return regionManager.Regions[RegionNames.DbSettingsRegion];
        }

        private DbSettingsView dbSettingsView;
    }
}
