using System.Collections.Generic;
using BusinessObjects;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Prism.Wizards;

namespace Modules.Import.ViewModels.Wizard.Common
{
    public abstract class WizardViewModelBase : ViewModelBase
    {
        public WizardViewModelBase(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
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
    }

    public class Data
    {
        public string ScanPath { get; set; }
        public TagsProvider TagsProvider { get; set; }
        public IEnumerable<Artist> ScannedArtists { get; set; }
    }
}
