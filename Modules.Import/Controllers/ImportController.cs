using System.Windows;
using Common;
using Common.Controllers;
using Common.Enums;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Import.Events;
using Modules.Import.ViewModels.Wizard;
using Modules.Import.Views;
using Modules.Import.Views.Wizard;
using Prism.Wizards;

namespace Modules.Import.Controllers
{
    public class ImportController : WorkspaceControllerBase
    {
        public ImportController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator)
        {
            RegisterImportViews();

            DisplayImportView(ViewNames.ImportView);

            eventAggregator.GetEvent<BeginScanProgressEvent>().Subscribe(OnDisplayImportProgressViewEvent, true);

            //obsolete
            eventAggregator.GetEvent<CompleteScanProgressEvent>().Subscribe(OnCompleteOrCancelScanProgressEvent, true);
            eventAggregator.GetEvent<CancelScanProgressEvent>().Subscribe(OnCompleteOrCancelScanProgressEvent, true);
        }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<ImportRegionView>(), WorkspaceNameEnum.Import.ToString());
        }

        private void RegisterImportViews()
        {
            IRegion importRegion = GetImportRegion();
            importRegion.Add(container.Resolve<ImportView>(), ViewNames.ImportView);
            importRegion.Add(container.Resolve<ImportProgressView>(), ViewNames.ImportProgressView);
        }

        private void OnDisplayImportProgressViewEvent(object parameter)
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

            var w = new Wizard(container, settings, "wizard");
            w.Start();
            //DisplayImportView(ViewNames.ImportProgressView);
        }

        private void OnCompleteOrCancelScanProgressEvent(object parameter)
        {
            DisplayImportView(ViewNames.ImportView);
        }

        #region Helpers

        private IRegion GetImportRegion()
        {
            return regionManager.Regions[RegionNames.ImportRegion];
        }

        private void DisplayImportView(string viewName)
        {
            IRegion importRegion = GetImportRegion();

            object view = importRegion.GetView(viewName);
            if (view != null && !IsActiveView(importRegion, view))
            {
                importRegion.Activate(view);
            }
        }

        private bool IsActiveView(IRegion region, object view)
        {
            return region.ActiveViews.Contains(view);
        }

        #endregion
    }
}
