using Common.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace Modules.Import.ViewModels
{
    public class ImportProgressViewModel : DialogViewModelBase, IImportProgressViewModel
    {
        public ImportProgressViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator)
            : base(unityContainer, eventAggregator)
        {
        }

        public override void OnSuccessCommand(object parameter)
        {
            DialogResult = true;
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

        public void Init()
        {
            ScanFilesCount = 0;
            ScannedFilesCount = 0;
        }

        public void NotifyFileScanned()
        {
            ScannedFilesCount += 1;
        }

        #endregion

        #region Private fields

        private int scanFilesCount;
        private int scannedFilesCount;

        #endregion
    }
}
