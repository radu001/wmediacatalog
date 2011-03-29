using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.ViewModels;
using FolderPickerLib;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services;

namespace Modules.Import.ViewModels
{
    public class ImportProgressViewModel : DialogViewModelBase, IImportProgressViewModel
    {
        public ImportProgressViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator, IDataService dataService)
            : base(unityContainer, eventAggregator)
        {
            this.dataService = dataService;

            SelectScanPathCommand = new DelegateCommand<object>(OnSelectScanPathCommand);
            BeginScanCommand = new DelegateCommand<object>(OnBeginScanCommand);
        }

        public override void OnSuccessCommand(object parameter)
        {
            BeginScanCommand.Execute(null);
            //DialogResult = true;
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        #region IImportProgressViewModel Members

        public int ScanFilesCount
        {
            get
            {
                return scanFilesCount;
            }
            set
            {
                scanFilesCount = value;
                NotifyPropertyChanged(() => ScanFilesCount);
            }
        }

        public int ScannedFilesCount
        {
            get
            {
                return scannedFilesCount;
            }
            private set
            {
                scannedFilesCount = value;
                NotifyPropertyChanged(() => ScannedFilesCount);
            }
        }

        public string ScanPath
        {
            get
            {
                return scanPath;
            }
            private set
            {
                scanPath = value;
                NotifyPropertyChanged(() => ScanPath);
            }
        }

        public ObservableCollection<Artist> Artists
        {
            get
            {
                return artists;
            }
            private set
            {
                artists = value;
                NotifyPropertyChanged(() => Artists);
            }
        }

        public double CurrentProgress
        {
            get
            {
                return currentProgress;
            }
            private set
            {
                currentProgress = value;
                NotifyPropertyChanged(() => CurrentProgress);
            }
        }

        public DelegateCommand<object> SelectScanPathCommand { get; private set; }

        public DelegateCommand<object> BeginScanCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnSelectScanPathCommand(object parameter)
        {
            FolderPickerDialog dialog = new FolderPickerDialog();
            if (dialog.ShowDialog() == true)
            {
                ScanPath = dialog.SelectedPath;
            }
        }

        private void OnBeginScanCommand(object parameter)
        {
            if (String.IsNullOrEmpty(ScanPath))
                Notify("Please select scan path first", NotificationType.Warning);
            else
            {
                var settings = CreateScanSettings();

                Init();

                Task<IEnumerable<Artist>> scanTask =
                    Task.Factory.StartNew<IEnumerable<Artist>>(() =>
                    {
                        return dataService.BeginScan(settings);
                    }, TaskScheduler.Default);

                Task finishTask = scanTask.ContinueWith((l) =>
                {
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void Init()
        {
            ScanFilesCount = 0;
            ScannedFilesCount = 0;
            CurrentProgress = 0;
        }

        private ScanSettings CreateScanSettings()
        {
            return new ScanSettings()
            {
                ScanPath = ScanPath,
                FileMask = "*.flac",
                BeginFileScan = OnBeginFileScan,
                BeginDirectoryScan = OnBeginDirectoryScan,
                BeforeScan = OnBeforeScan
            };
        }

        #region Scan progress handlers

        private void OnBeforeScan(int filesCount)
        {
            ScanFilesCount = filesCount;
            step = 100d / ScanFilesCount;
        }

        private void OnBeginFileScan(string fileName)
        {
            CurrentProgress += step;
        }

        private void OnBeginDirectoryScan(string pathName)
        {
        }

        #endregion

        #endregion

        #region Private fields

        private IDataService dataService;
        private ObservableCollection<Artist> artists;
        private int scanFilesCount;
        private int scannedFilesCount;
        private string scanPath;
        private double currentProgress;

        private double step;

        #endregion
    }
}
