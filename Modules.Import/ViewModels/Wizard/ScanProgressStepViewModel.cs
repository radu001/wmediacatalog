
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services;
using Modules.Import.ViewModels.Wizard.Common;
using Prism.Wizards.Events;
namespace Modules.Import.ViewModels.Wizard
{
    public class ScanProgressStepViewModel : WizardViewModelBase, IScanProgressStepViewModel
    {
        public ScanProgressStepViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            BeginScanCommand = new DelegateCommand<object>(OnBeginScanCommand);
            PauseScanCommand = new DelegateCommand<object>(OnPauseScanCommand);
            CancelScanCommand = new DelegateCommand<object>(OnCancelScanCommand);
        }

        public override void OnContinueCommand(object parameter)
        {
            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        #region IScanProgressStepViewModel Members

        public string ScanPath { get; private set; }

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

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
            private set
            {
                isCompleted = value;
                NotifyPropertyChanged(() => IsCompleted);
            }
        }

        public int ScanFilesCount
        {
            get
            {
                return scanFilesCount;
            }
            private set
            {
                scanFilesCount = value;
                NotifyPropertyChanged(() => ScanFilesCount);
            }
        }

        public float MinProgress
        {
            get
            {
                return 0f;
            }
        }

        public float MaxProgress
        {
            get
            {
                return 100f;
            }
        }

        public float CurrentProgress
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

        public StringBuilder Log
        {
            get
            {
                return log;
            }
            private set
            {
                log = value;
                NotifyPropertyChanged(() => Log);
            }
        }

        public DelegateCommand<object> BeginScanCommand { get; private set; }

        public DelegateCommand<object> PauseScanCommand { get; private set; }

        public DelegateCommand<object> CancelScanCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnBeginScanCommand(object parameter)
        {
            var data = GetSharedData();
            ScanPath = data.ScanPath;

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

                    CompleteScan(l.Result);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void OnPauseScanCommand(object parameter)
        {
            if (IsScanning && !IsPaused && !IsCompleted && scanSettings != null)
            {
                IsPaused = true;
                scanSettings.Pause = true;
            }
            else
            {
                if (IsScanning && IsPaused && !IsCompleted)
                {
                    IsPaused = false;
                    scanSettings.Pause = false;
                }
            }
        }

        private void OnCancelScanCommand(object parameter)
        {
        }

        private ScanSettings CreateScanSettings()
        {
            return new ScanSettings()
            {
                ScanPath = ScanPath,
                FileMasks = new string[] { "*.flac" }, // todo from provider
                BeginFileScan = OnBeginFileScan,
                BeginDirectoryScan = OnBeginDirectoryScan,
                BeforeScan = OnBeforeScan
            };
        }

        private void Init()
        {
            IsCompleted = false;
            ScanFilesCount = 0;
            CurrentProgress = MinProgress;
            Log = new StringBuilder();
        }

        private void CompleteScan(IEnumerable<Artist> artists)
        {
            IsCompleted = true;
            var data = GetSharedData();
            data.ScannedArtists = artists;
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
            Log.AppendLine(pathName);
            NotifyPropertyChanged(() => Log);
        }

        #endregion

        #endregion

        #region Private fields

        private IDataService dataService;
        private bool isScanning;
        private bool isPaused;
        private bool isCompleted;

        private int scanFilesCount;
        private float step;
        private float currentProgress;
        private StringBuilder log;

        private ScanSettings scanSettings;

        #endregion
    }
}
