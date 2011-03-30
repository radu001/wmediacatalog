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
    public class ImportProgressViewModel : ViewModelBase, IImportProgressViewModel
    {
        public ImportProgressViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator, IDataService dataService)
            : base(unityContainer, eventAggregator)
        {
            this.dataService = dataService;

            SelectScanPathCommand = new DelegateCommand<object>(OnSelectScanPathCommand);
            BeginScanCommand = new DelegateCommand<object>(OnBeginScanCommand);
            PauseScanCommand = new DelegateCommand<object>(OnPauseScanCommand);
            CancelScanCommand = new DelegateCommand<object>(OnCancelScanCommand);
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

        public bool IsScanning
        {
            get
            {
                return isScanning;
            }
            private set
            {
                isScanning = value;
                NotifyPropertyChanged(() => IsScanning);
            }
        }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            private set
            {
                isPaused = value;
                NotifyPropertyChanged(() => IsPaused);
            }
        }

        public DelegateCommand<object> SelectScanPathCommand { get; private set; }

        public DelegateCommand<object> BeginScanCommand { get; private set; }

        public DelegateCommand<object> PauseScanCommand { get; private set; }

        public DelegateCommand<object> CancelScanCommand { get; private set; }

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
                scanSettings = CreateScanSettings();

                Init();

                IsScanning = true;
                IsPaused = false;

                Task<IEnumerable<Artist>> scanTask =
                    Task.Factory.StartNew<IEnumerable<Artist>>(() =>
                    {
                        return dataService.BeginScan(scanSettings);
                    }, TaskScheduler.Default);

                Task finishTask = scanTask.ContinueWith((l) =>
                {
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void OnPauseScanCommand(object parameter)
        {
            if (!IsScanning)
                return;
            else
            {
                scanSettings.Pause = true;
                IsPaused = true;
            }
        }

        private void OnCancelScanCommand(object parameter)
        {
            if (!IsScanning)
            {
                //todo return to import region using event and controller
            }
            else
            {
                //show confirmation
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
        private bool isScanning;
        private bool isPaused;

        private double step;
        private ScanSettings scanSettings;

        #endregion
    }
}
