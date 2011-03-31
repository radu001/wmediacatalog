using System.Collections.Generic;
using System.Xml.Linq;
using BusinessObjects;
using Common.Enums;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Events;
using Modules.Import.Model;
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
            MatchAgainstDatabaseCommand = new DelegateCommand<object>(OnMatchAgainstDatabaseCommand);

            eventAggregator.GetEvent<CompleteScanProgressEvent>().Subscribe(OnCompleteScanProgressEvent, true);

            XElement root = new XElement("genres");
            for (int i = 1; i < 11; ++i)
            {
                root.Add(new XElement("g", "genre" + i.ToString()));
            }
        }

        #region IImportViewModel Members

        public IImportDataModel DataModel
        {
            get
            {
                return dataModel;
            }
            private set
            {
                dataModel = value;
                NotifyPropertyChanged(() => DataModel);
            }
        }

        public DelegateCommand<object> ScanFilesCommand { get; private set; }

        public DelegateCommand<object> MatchAgainstDatabaseCommand { get; private set; }

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

            DataModel = CreateDataModel(artists);

            //debug
            MatchAgainstDatabaseCommand.Execute(null);
        }

        private void OnMatchAgainstDatabaseCommand(object parameter)
        {
            if (dataService.BulkImportData(DataModel))
                Notify("Bulk data import has been successful", NotificationType.Success);
            else
                Notify("Error during bulk data import. See log for details", NotificationType.Error);
        }

        private IImportDataModel CreateDataModel(IEnumerable<Artist> artists)
        {
            return new ImportDataModel(artists);
        }

        #endregion

        #region Private fields

        private readonly IDataService dataService;
        private IImportDataModel dataModel;

        #endregion
    }
}
