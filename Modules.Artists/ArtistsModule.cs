using Common.Controllers;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Artists.Controllers;
using Modules.Artists.Services;
using Modules.Artists.ViewModels;

namespace Modules.Artists
{
    public class ArtistsModule : IModule
    {
        public ArtistsModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IArtistsViewModel, ArtistsViewModel>();
            container.RegisterType<IArtistEditViewModel, ArtistEditViewModel>();

            artistsController = container.Resolve<ArtistsController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private WorkspaceControllerBase artistsController;
    }
}
