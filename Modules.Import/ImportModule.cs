using Common;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Import.Controllers;
using Modules.Import.Services;
using Modules.Import.ViewModels;
using Modules.Import.Views;

namespace Modules.Import
{
    public class ImportModule : IModule
    {
        public ImportModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IImportViewModel, ImportViewModel>();
            //container.RegisterType<IArtistEditViewModel, ArtistEditViewModel>();

            importController = container.Resolve<ImportController>();

            regionManager.RegisterViewWithRegion(RegionNames.ImportRegion, typeof(ImportView));
            regionManager.RegisterViewWithRegion(RegionNames.ExportRegion, typeof(ExportView));
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private ImportController importController;
    }
}
