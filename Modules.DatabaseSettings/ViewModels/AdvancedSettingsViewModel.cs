using System;
using System.IO;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Modules.DatabaseSettings.Data;
using Modules.DatabaseSettings.Services;
using Modules.DatabaseSettings.Utils;
using Modules.DatabaseSettings.Utils.Psql;

namespace Modules.DatabaseSettings.ViewModels
{
    public class AdvancedSettingsViewModel : ViewModelBase, IAdvancedSettingsViewModel
    {
        public AdvancedSettingsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            LocatePsqlCommand = new DelegateCommand<object>(OnLocatePsqlCommand);
            CancelLocatePsqlCommand = new DelegateCommand<object>(OnCancelLocatePsqlCommand);
            SetPsqlSearchPathCommand = new DelegateCommand<object>(OnSetPsqlSearchPathCommand);
            ValidateSchemaCommand = new DelegateCommand<object>(OnValidateSchemaCommand);
            DeploySchemaCommand = new DelegateCommand<object>(OnDeploySchemaCommand);
            BasicSettingsCommand = new DelegateCommand<object>(OnBasicSettingsCommand);

            DatabaseDeployInfo = new DatabaseDeployInfo();
            PsqlLocator = new PsqlLocator();
        }

        #region IAdvancedSettingsViewModel Members

        public DatabaseDeployInfo DatabaseDeployInfo
        {
            get
            {
                return dbDeployInfo;
            }
            set
            {
                dbDeployInfo = value;
                NotifyPropertyChanged(() => DatabaseDeployInfo);
            }
        }

        public IPsqlLocator PsqlLocator
        {
            get
            {
                return psqlLocator;
            }
            set
            {
                psqlLocator = value;
                NotifyPropertyChanged(() => PsqlLocator);
            }
        }

        public bool PsqlLocatorDetailsVisible
        {
            get
            {
                return psqlLocatorDetailsVisible;
            }
            set
            {
                psqlLocatorDetailsVisible = value;
                NotifyPropertyChanged(() => PsqlLocatorDetailsVisible);
            }
        }

        public DelegateCommand<object> LocatePsqlCommand { get; private set; }

        public DelegateCommand<object> CancelLocatePsqlCommand { get; private set; }

        public DelegateCommand<object> SetPsqlSearchPathCommand { get; private set; }

        public DelegateCommand<object> ValidateSchemaCommand { get; private set; }

        public DelegateCommand<object> DeploySchemaCommand { get; private set; }

        public DelegateCommand<object> BasicSettingsCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnLocatePsqlCommand(object parameter)
        {
            PsqlLocator.BeginLocate(OnPsqlLocateCompleted);
        }

        private void OnPsqlLocateCompleted(FileInfo psqlFile)
        {
            if (psqlFile != null)
            {
                Notify(String.Format("Found psql binary: {0}", psqlFile.DirectoryName), NotificationType.Success);

                DatabaseDeployInfo.PsqlPath = psqlFile.FullName;
                PsqlLocatorDetailsVisible = false;
            }
            else
            {
                Notify
                    (
                    String.Format("Can't find psql from given path: {0}. Please try another path", PsqlLocator.InitialPath), 
                    NotificationType.Warning
                    );
            }
        }

        private void OnCancelLocatePsqlCommand(object parameter)
        {
            PsqlLocator.CancelLocate();
        }

        private void OnSetPsqlSearchPathCommand(object parameter)
        {
            //TODO change PsqlLocator.Initial path using directory browse dialog
        }

        private void OnValidateSchemaCommand(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            if (!validator.Validate(parameter))
            {
                Notify("Please fill all required fields", NotificationType.Error);
            }
            else
            {
                IPsqlShell shell = new PsqlShell( new FileInfo(DatabaseDeployInfo.PsqlPath));
                //TODO Init psqlShell and try to launch psql executable using given root password and username
            }
        }


        private void OnDeploySchemaCommand(object parameter)
        {
        }

        private void OnBasicSettingsCommand(object parameter)
        {
            eventAggregator.GetEvent<SwitchDbSettingsViewEvent>().Publish(DbSettingsMode.ConnectionSettings);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private DatabaseDeployInfo dbDeployInfo;

        private IPsqlLocator psqlLocator;
        private bool psqlLocatorDetailsVisible;

        #endregion


    }
}
