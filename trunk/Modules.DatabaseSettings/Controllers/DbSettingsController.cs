
using System.Windows;
using Common;
using Common.Events;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.DatabaseSettings.Views;
namespace Modules.DatabaseSettings.Controllers
{
    public class DbSettingsController
    {
        public DbSettingsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            dbSettingsView = new DbSettingsView();
            RegionManager.SetRegionManager(dbSettingsView, regionManager);

            InitViews();

            eventAggregator.GetEvent<SetupDatabaseEvent>().Subscribe(OnSetupDatabaseEvent, true);
            eventAggregator.GetEvent<SwitchDbSettingsViewEvent>().Subscribe(OnSwitchAdvancedDbSettingsEvent, true);
        }

        private void OnSetupDatabaseEvent(object parameter)
        {
            DisplayView(ViewNames.DbConnectionSettingsView);
        }

        private void OnSwitchAdvancedDbSettingsEvent(DbSettingsMode payLoad)
        {
            switch (payLoad)
            {
                case DbSettingsMode.AdvancedSettings:
                    DisplayView(ViewNames.DbAdvancedSettingsView);
                    break;
                case DbSettingsMode.ConnectionSettings:
                    DisplayView(ViewNames.DbConnectionSettingsView);
                    break;
            }
        }

        private void InitViews()
        {
            IRegion mainRegion = GetDbSettingsRegion();
            mainRegion.Add(container.Resolve<ConnectionSettingsView>(), ViewNames.DbConnectionSettingsView);
            mainRegion.Add(container.Resolve<AdvancedSettingsView>(), ViewNames.DbAdvancedSettingsView);
        }

        private IRegion GetDbSettingsRegion()
        {
            return regionManager.Regions[RegionNames.DbSettingsRegion];
        }

        private void DisplayView(string viewName)
        {
            IRegion mainRegion = GetDbSettingsRegion();
            object view = mainRegion.GetView(viewName);

            if (view != null)
            {
                if (dbSettingsView.Visibility == Visibility.Collapsed || dbSettingsView.Visibility == Visibility.Hidden)
                    dbSettingsView.ShowDialog();
                mainRegion.Activate(view);
            }
        }

        private IUnityContainer container;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        private DbSettingsView dbSettingsView;
    }
}
