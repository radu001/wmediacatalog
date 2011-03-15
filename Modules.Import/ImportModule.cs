using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Import.Services;
using Modules.Import.ViewModels;
using Modules.Import.Controllers;

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
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private ImportController importController;
    }
}
