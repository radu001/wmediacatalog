using System;
using System.Threading.Tasks;
using Common.Entities;
using Common.Enums;
using Common.ViewModels;
using DataServices.Additional;
using FolderPickerLib;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
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
            SelectExportPathCommand = new DelegateCommand<object>(OnSelectExportPathCommand);
            SelectProviderPathCommand = new DelegateCommand<object>(OnSelectProviderPathCommand);

            SelectBackupCommand = new DelegateCommand<object>(OnSelectBackupCommand);
            ImportDatabaseCommand = new DelegateCommand<object>(OnImportDatabaseCommand);

            InitDefaultSettings();
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

        public string BackupFullPath
        {
            get
            {
                return backupFullPath;
            }
            set
            {
                backupFullPath = value;
                NotifyPropertyChanged(() => BackupFullPath);
            }
        }

        public DelegateCommand<object> ExportDatabaseCommand { get; private set; }

        public DelegateCommand<object> SelectProviderPathCommand { get; private set; }

        public DelegateCommand<object> SelectExportPathCommand { get; private set; }

        public DelegateCommand<object> SelectBackupCommand { get; private set; }

        public DelegateCommand<object> ImportDatabaseCommand { get; private set; }

        #endregion

        #region Private methods

        #region Export

        private void OnExportDatabaseCommand(object parameter)
        {
            var settings = CreateExportSettings();
            var validationResult = dataService.ValidateExportSettings(settings);

            if (!validationResult.Success)
                Notify(String.Format("Illegal export settings. {0}", validationResult.Message), NotificationType.Error);
            else
            {
                IsBusy = true;

                Task<TextResult> exportTask = Task.Factory.StartNew<TextResult>(() =>
                {
                    return dataService.Export(settings);
                }, TaskScheduler.Default);

                Task finishExportTask = exportTask.ContinueWith((t) =>
                {
                    IsBusy = false;
                    OnExportCompleted(t.Result);
                },TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void OnSelectProviderPathCommand(object parameter)
        {
            var dialog = new FolderPickerDialog()
            {
                InitialPath = @"I:\Graphics\Tiffany Toth\PB- CGOM March 2006\02"
            };
            if (dialog.ShowDialog() == true)
            {
                ProviderPath = dialog.SelectedPath;
            }
        }

        private void OnSelectExportPathCommand(object parameter)
        {
            var dialog = new FolderPickerDialog();
            if (dialog.ShowDialog() == true)
            {
                ExportPath = dialog.SelectedPath;
            }
        }

        private void InitDefaultSettings()
        {
            UserName = "user";
            Password = "password";
            ProviderPath = @"C:\Program Files\PostgreSQL\8.4\bin";
            ExportPath = @"D:\";
            ExportFileName = "backup.db";
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

        private void OnExportCompleted(TextResult result)
        {
            IsBusy = false;
            if (result.Success)
            {
                Notify("Database has been successfully exported", NotificationType.Success);
            }
            else
            {
                Notify(
                    String.Format( "Error while exporting database: [{0}]", result.Message),
                    NotificationType.Error);
            }
        }

        #endregion

        #region Import

        private void OnSelectBackupCommand(object parameter)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                BackupFullPath = fileDialog.FileName;
            }
        }

        private void OnImportDatabaseCommand(object parameter)
        {
            var settings = CreateImportSettings();
            var validationResult = dataService.ValidateImportSettings(settings);

            if (!validationResult.Success)
                Notify(String.Format("Illegal import settings. {0}", validationResult.Message), NotificationType.Error);
            else
            {
                IsBusy = true;

                Task<TextResult> exportTask = Task.Factory.StartNew<TextResult>(() =>
                {
                    return dataService.Import(settings);
                }, TaskScheduler.Default);

                Task finishExportTask = exportTask.ContinueWith((t) =>
                {
                    IsBusy = false;
                    OnImportCompleted(t.Result);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private ImportProviderSettings CreateImportSettings()
        {
            return new ImportProviderSettings()
            {
                BackupFullName = BackupFullPath,
                ProviderPath = ProviderPath,
                UserName = UserName,
                Password = Password
            };
        }

        private void OnImportCompleted(TextResult result)
        {
            IsBusy = false;
            if (result.Success)
            {
                Notify("Database has been successfully imported. It is recommended to restart " + 
                       "application in order to apply data changes", NotificationType.Success);
            }
            else
            {
                Notify(
                    String.Format("Error while importing database: [{0}]", result.Message),
                    NotificationType.Error);
            }
        }

        #endregion

        #endregion

        #region Private fields

        private IDataService dataService;

        private string userName;
        private string password;
        private string providerPath;
        private string exportPath;
        private string exportFileName;
        private string backupFullPath;

        #endregion
    }
}
