using Common.Controllers;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Albums.Controllers;
using Modules.Albums.Services;
using Modules.Albums.ViewModels;

namespace Modules.Albums
{
    public class AlbumsModule : IModule
    {
        public AlbumsModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IGenresListViewModel, GenresListViewModel>();
            container.RegisterType<IGenreEditViewModel, GenreEditViewModel>();
            container.RegisterType<IAlbumsViewModel, AlbumsViewModel>();
            container.RegisterType<IAlbumSearchViewModel, AlbumSearchViewModel>();
            container.RegisterType<IArtistListViewModel, ArtistsListViewModel>();
            container.RegisterType<ITracksListViewModel, TracksListViewModel>();
            container.RegisterType<IAlbumEditViewModel, AlbumEditViewModel>();

            albumsController = container.Resolve<AlbumsController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private WorkspaceControllerBase albumsController;
    }
}
