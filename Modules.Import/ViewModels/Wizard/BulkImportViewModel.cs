using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Services;
using Modules.Import.ViewModels.Wizard.Common;
using Prism.Wizards.Events;

namespace Modules.Import.ViewModels.Wizard
{
    public class BulkImportViewModel : WizardViewModelBase, IBulkImportViewModel
    {
        public BulkImportViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            BeginImportCommand = new DelegateCommand<object>(OnBeginImportCommand);
            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
        }

        public override void OnContinueCommand(object parameter)
        {
            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        #region IBulkImportViewModel Members

        public bool ImportCompleted
        {
            get
            {
                return importCompleted;
            }
            private set
            {
                importCompleted = value;
                NotifyPropertyChanged(() => ImportCompleted);
            }
        }

        public IEnumerable<Artist> Artists
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

        public DelegateCommand<object> BeginImportCommand { get; private set; }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnBeginImportCommand(object parameter)
        {
            IsBusy = true;

            Task<bool> importTask = Task.Factory.StartNew<bool>(() =>
            {
                return dataService.BulkImportData(Artists);
            }, TaskScheduler.Default);

            Task finishImportTask = importTask.ContinueWith((r) =>
            {
                IsBusy = false;

                if (r.Result)
                {
                    Notify("Import has been successful", NotificationType.Success);

                    ImportCompleted = true;

                    eventAggregator.GetEvent<ReloadAlbumsEvent>().Publish(null);
                    eventAggregator.GetEvent<ReloadArtistsEvent>().Publish(null);

                    //automatically navigate to next step in wizard
                    ContinueCommand.Execute(null);
                }
                else
                {
                    Notify("Errors occured during import. Please see log for details", NotificationType.Error);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnViewLoadedCommand(object parameter)
        {
            PrepareImportData();
        }

        private void PrepareImportData()
        {
            var data = GetSharedData();
            Artists = data.ScannedArtists;
        }

        #endregion

        #region Private fields

        private bool importCompleted;
        private IEnumerable<Artist> artists;
        private IDataService dataService;

        #endregion
    }
}
