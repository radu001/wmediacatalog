using System.Collections.Generic;
using System.Collections.ObjectModel;
using BusinessObjects;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Events;
using Modules.Import.Services;

namespace Modules.Import.ViewModels
{
    public class ImportViewModel : ViewModelBase, IImportViewModel
    {
        #region Properties

        public int ScanFilesCount { get; private set; }

        public int ScannedFilesCount { get; private set; }

        #endregion

        public ImportViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            ScanFilesCommand = new DelegateCommand<object>(OnScanFilesCommand);

            eventAggregator.GetEvent<CompleteScanProgressEvent>().Subscribe(OnCompleteScanProgressEvent, true);
        }

        #region IImportViewModel Members

        public DelegateCommand<object> ScanFilesCommand { get; private set; }

        public ObservableCollection<Artist> ImportedArtists
        {
            get
            {
                return importedArtists;
            }
            private set
            {
                importedArtists = value;
                NotifyPropertyChanged(() => ImportedArtists);
            }
        }

        #endregion

        #region Private methods

        private void OnScanFilesCommand(object parameter)
        {
            eventAggregator.GetEvent<BeginScanProgressEvent>().Publish(null);
        }

        private void OnCompleteScanProgressEvent(IEnumerable<Artist> artists)
        {
            if (artists == null)
                return;

            ImportedArtists = new ObservableCollection<Artist>(artists);
        }

        #endregion

        #region Private fields

        private readonly IDataService dataService;
        private ObservableCollection<Artist> importedArtists;

        #endregion
    }
}
