using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Dialogs;
using Common.Enums;
using Common.ViewModels;
using FolderPickerLib;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Events;
using Modules.Import.Model;
using Modules.Import.Services;

namespace Modules.Import.ViewModels
{
    public class ImportProgressViewModel : ViewModelBase, IImportProgressViewModel
    {
        public const double MinProgress = 0;
        public const double MaxProgress = 100;

        public ImportProgressViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator, IDataService dataService)
            : base(unityContainer, eventAggregator)
        {
            this.dataService = dataService;

            Artists = new Artist[] { };

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

        public IEnumerable<Artist> Artists { get; private set; }

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

        public StringBuilder LogText
        {
            get
            {
                return logText;
            }
            private set
            {
                logText = value;
                NotifyPropertyChanged(() => LogText);
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
                    IsScanning = false;
                    IsPaused = false;
                    CurrentProgress = MaxProgress;

                    Artists = l.Result;

                    CompleteScan();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void OnPauseScanCommand(object parameter)
        {
            if (!IsScanning)
                return;
            else
            {
                if (!IsPaused)
                {
                    scanSettings.Pause = true;
                    IsPaused = true;
                }
                else
                {
                    scanSettings.Pause = false;
                    IsPaused = false;
                }
            }
        }

        private void OnCancelScanCommand(object parameter)
        {
            if (!IsScanning)
            {
                CompleteScan();
            }
            else
            {
                ConfirmDialog confirm = new ConfirmDialog()
                {
                    MessageText = "Do you really want to cancel scanning progress?",
                    HeaderText = "Confirm"
                };

                if (confirm.ShowDialog() == true)
                {
                    /* this will cause data service to finish scan task and control flow
                     * will return to first condition in this method*/
                    isCanceled = true;
                    scanSettings.Stop = true;
                }
            }
        }

        private void Init()
        {
            ScanFilesCount = 0;
            ScannedFilesCount = 0;
            CurrentProgress = MinProgress;
            LogText = new StringBuilder();
        }

        private void CompleteScan()
        {
            if (!isCanceled)
            {
                int artistCount = Artists.Count();
                int albumsCount = Artists.SelectMany(a => a.Albums).Count();

                Notify(String.Format("Scanning has been completed. Found {0} artists, {1} albums", artistCount, albumsCount),
                       NotificationType.Success);

                eventAggregator.GetEvent<CompleteScanProgressEvent>().Publish(Artists);
            }
            else
            {
                eventAggregator.GetEvent<CancelScanProgressEvent>().Publish(null);
            }
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
            step = MaxProgress / ScanFilesCount;
        }

        private void OnBeginFileScan(string fileName)
        {
            CurrentProgress += step;
        }

        private void OnBeginDirectoryScan(string pathName)
        {
            LogText.AppendLine(pathName);
            NotifyPropertyChanged(() => LogText);
        }

        #endregion

        #endregion

        #region Private fields

        private IDataService dataService;
        private int scanFilesCount;
        private int scannedFilesCount;
        private string scanPath;
        private double currentProgress;
        private bool isScanning;
        private bool isPaused;
        private StringBuilder logText;

        private double step;
        private bool isCanceled;

        private ScanSettings scanSettings;


        #endregion
    }
}
