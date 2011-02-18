
using System;
using System.IO;
using System.Threading.Tasks;
using Common.Dialogs;
using Common.Enums;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Modules.DatabaseSettings.Data;
using Modules.DatabaseSettings.Services;
namespace Modules.DatabaseSettings.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase, IConnectionSettingsViewModel
    {
        public ConnectionSettingsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            TestConfigurationCommand = new DelegateCommand<object>(OnTestConfigurationCommand);
            SaveConfigurationCommand = new DelegateCommand<object>(OnSaveConfigurationCommand);
            AdvancedSettingsCommand = new DelegateCommand<object>(OnAdvancedSettingsCommand);
        }

        #region IConnectionSettingsViewModel Members


        public INHibernateConfig NHibernateConfig
        {
            get
            {
                return config;
            }
            private set
            {
                config = value;
                NotifyPropertyChanged(() => NHibernateConfig);
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> TestConfigurationCommand { get; private set; }

        public DelegateCommand<object> SaveConfigurationCommand { get; private set; }

        public DelegateCommand<object> AdvancedSettingsCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnViewLoadedCommand(object parameter)
        {
            INHibernateConfig config = new NHibernateConfigModel();
            if (config.Load(ConfigFileName))
            {
                NHibernateConfig = config;
            }
            else
            {
                Notify(String.Format("Can't load NHibernate configuration from {0}", ConfigFileName), NotificationType.Error);
            }
        }

        private void OnTestConfigurationCommand(object parameter)
        {
            if (NHibernateConfig == null)
            {
                Notify("Config isn't loaded", NotificationType.Error);
                return;
            }

            IsBusy = true;

            Task<Exception> validateDatabaseTask = Task.Factory.StartNew<Exception>(() =>
            {
                return dataService.ValidateConnection();
            }, TaskScheduler.Default);

            Task finishTask = validateDatabaseTask.ContinueWith((t) =>
            {
                IsBusy = false;

                Exception connectionResult = t.Result;

                if (connectionResult == null)
                {
                    Notify("Successfully connected to database", NotificationType.Success);
                }
                else
                {
                    Notify(String.Format("Error: {0}", connectionResult.Message), NotificationType.Error);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void OnSaveConfigurationCommand(object parameter)
        {
            if (NHibernateConfig == null)
            {
                Notify("Config isn't loaded", NotificationType.Error);
                return;
            }

            ConfirmDialog dialog = new ConfirmDialog()
            {
                HeaderText = "Artist remove confirmation",
                MessageText = "Do you really want to save configuration? Backup will be create automatically"
            };

            if (dialog.ShowDialog() == true)
            {
                //backup old config
                DirectoryInfo dir = new DirectoryInfo(BackupDir);
                if (!dir.Exists)
                    dir.Create();

                string newName = "backup" + DateTime.Now.ToBinary().ToString() + '-' + ConfigFileName;
                string fullPath = BackupDir + "\\" + newName;
                File.Copy(ConfigFileName, fullPath, true);

                if (NHibernateConfig.Save(ConfigFileName))
                    Notify(
                        String.Format("Configuration has been successfully saved. Created backup {0}", newName),
                        NotificationType.Success);
                else
                    Notify("Can't save configuration. Unexpected error", NotificationType.Error);
            }
        }

        private void OnAdvancedSettingsCommand(object parameter)
        {
            eventAggregator.GetEvent<SwitchDbSettingsViewEvent>().Publish(DbSettingsMode.AdvancedSettings);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private INHibernateConfig config;

        private static readonly string ConfigFileName = "hibernate.cfg.xml";
        private static readonly string BackupDir = "backup";

        #endregion
    }
}
