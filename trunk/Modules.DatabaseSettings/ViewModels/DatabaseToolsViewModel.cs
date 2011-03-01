using System;
using Common.Entities;
using Common.Enums;
using Common.ViewModels;
using DataServices.Additional;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Modules.DatabaseSettings.Services;

namespace Modules.DatabaseSettings.ViewModels
{
    public class DatabaseToolsViewModel : ViewModelBase, IDatabaseToolsViewModel
    {
        public DatabaseToolsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            ExportDatabaseCommand = new DelegateCommand<object>(OnExportDatabaseCommand);
        }

        #region IDatabaseToolsViewModel Members

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                NotifyPropertyChanged(() => UserName);
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                NotifyPropertyChanged(() => Password);
            }
        }

        public string ProviderPath
        {
            get
            {
                return providerPath;
            }
            set
            {
                providerPath = value;
                NotifyPropertyChanged(() => ProviderPath);
            }
        }

        public string ExportPath
        {
            get
            {
                return exportPath;
            }
            set
            {
                exportPath = value;
                NotifyPropertyChanged(() => ExportPath);
            }
        }

        public string ExportFileName
        {
            get
            {
                return exportFileName;
            }
            set
            {
                exportFileName = value;
                NotifyPropertyChanged(() => ExportFileName);
            }
        }

        public DelegateCommand<object> ExportDatabaseCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnExportDatabaseCommand(object parameter)
        {
            var settings = CreateExportSettings();
            TextResult validationResult = dataService.ValidateProviderSettings(settings);
            if (!validationResult.Success)
            {
                Notify(String.Format("Illegal export settings: {0}", validationResult.Message), NotificationType.Error);
            }
            else
            {
                dataService.BeginExport(settings, OnExportCompleted);
            }
        }

        private ExportProviderSettings CreateExportSettings()
        {
            return new ExportProviderSettings()
            {
                ExportPath = ExportPath,
                ExportFileName = ExportFileName,
                ProviderPath = ProviderPath,
                UserName = UserName,
                Password = Password
            };
        }

        private void OnExportCompleted(bool success)
        {
        }

        #endregion

        #region Private fields

        private IDataService dataService;

        private string userName;
        private string password;
        private string providerPath;
        private string exportPath;
        private string exportFileName;

        #endregion
    }
}
