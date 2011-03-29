using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Dialogs;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Services;
using Modules.Import.Views;

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
            var viewModel = container.Resolve<IImportProgressViewModel>();
            var scanProgressDialog = new CommonDialog()
            {
                DialogContent = new ImportProgressView(viewModel)
            };

            if (scanProgressDialog.ShowDialog() == true)
            {
                //TODO
            }
        }

        #endregion

        #region Private fields

        private readonly IDataService dataService;
        private ObservableCollection<Artist> importedArtists;

        #endregion
    }
}
