using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Import.Controllers;
using Modules.Import.Services;
using Modules.Import.Services.Utils;
using Modules.Import.Services.Utils.FileSystem;
using Modules.Import.Services.Utils.Scanners;

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
            container.RegisterType<ITagsAccumulator, TagsAccumulator>();
            container.RegisterType<IScanner, CompositeScanner>();
            container.RegisterType<IFileSystem, FileSystem>();
            container.RegisterType<IFileSelector, FileByExtensionSelector>();

            importController = container.Resolve<ImportController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private ImportController importController;
    }
}
