using System.Windows;
using Common.Controllers;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Import.ViewModels.Wizard;
using Modules.Import.Views.Wizard;
using Prism.Wizards;

namespace Modules.Import.Controllers
{
    public class ImportController : ControllerBase
    {
        public ImportController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator)
        {
            eventAggregator.GetEvent<ImportDataEvent>().Subscribe(OnImportDataEvent, true);
        }

        private void OnImportDataEvent(object parameter)
        {
            var settings = new WizardSettings()
            {
                Size = new Size()
                {
                    Width = 640,
                    Height = 480
                }
            };
            settings.AddStep<IInitialStepViewModel,InitialStepViewModel,InitialStep>(0, "Initial");
            settings.AddStep<ITagsProviderStepViewModel, TagsProviderStepViewModel, TagsProviderStep>(1, "First");
            settings.AddStep<IScanProgressStepViewModel, ScanProgressStepViewModel, ScanProgressStep>(2, "Second");

            var w = new Wizard(container, settings, "wizard");
            w.Start();
        }
    }
}
