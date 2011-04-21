using Common.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Prism.Wizards;

namespace Modules.Import.ViewModels.Wizard.Common
{
    public abstract class WizardViewModelBase : NotificationObject
    {
        public WizardViewModelBase(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.container = container;

            ContinueCommand = new DelegateCommand<object>(OnContinueCommand);
        }

        protected Data GetSharedData()
        {
            var wizardSharedData = container.Resolve<IWizardData>();
            var typedData = wizardSharedData.GetValue<Data>();

            if (typedData == null)
            {
                typedData = new Data();
                wizardSharedData.SetValue<Data>(typedData);
            }

            return typedData;
        }

        public DelegateCommand<object> ContinueCommand { get; private set; }

        public abstract void OnContinueCommand(object parameter);

        private IUnityContainer container;
    }

    public class Data
    {
        public string ScanPath { get; set; }
        public TagsProvider TagsProvider { get; set; }
    }
}
