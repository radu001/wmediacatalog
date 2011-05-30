using BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Services;
using Modules.Import.ViewModels.Wizard.Common;
using Prism.Wizards.Events;

namespace Modules.Import.ViewModels.Wizard
{
    public class InitialStepViewModel : WizardViewModelBase, IInitialStepViewModel
    {
        public InitialStepViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
        }

        #region IInitialStepViewModel Members

        public string Message
        {
            get
            {
                return @"Welcome to import wizard. It will guide you through the process of " +
                         "importing data from your local storage(s) into media library";
            }
        }

        public bool IsStepDisabledNextTime
        {
            get
            {
                return isStepDisabledNextTime;
            }
            set
            {
                isStepDisabledNextTime = value;
                NotifyPropertyChanged(() => IsStepDisabledNextTime);
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnViewLoadedCommand(object parameter)
        {
            var settings = CurrentUser.GetSettings();

            IsStepDisabledNextTime = !settings.ImportFirstStepVisible;

            if (!settings.ImportFirstStepVisible)
            {
                ContinueCommand.Execute(null);
            }
        }

        private void ChangeImportDataWizardInitialStepVisibility()
        {
            var settings = CurrentUser.GetSettings();

            bool disableStep = !IsStepDisabledNextTime; // inverse

            //setting has been actually changed
            if (settings.ImportFirstStepVisible != disableStep)
            {
                settings.ImportFirstStepVisible = disableStep;
                dataService.SaveUserSettings();
            }
        }

        #endregion

        public override void OnContinueCommand(object parameter)
        {
            ChangeImportDataWizardInitialStepVisibility();

            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        #region Private fields

        private IDataService dataService;
        private bool isStepDisabledNextTime;

        #endregion
    }
}
