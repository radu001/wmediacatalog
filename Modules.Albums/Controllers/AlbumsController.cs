using Common.Controllers;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Albums.ViewModels;
using Modules.Albums.Views;

namespace Modules.Albums.Controllers
{
    public class AlbumsController : WorkspaceControllerBase
    {
        public AlbumsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator)
        {
            eventAggregator.GetEvent<SearchAlbumEvent>().Subscribe(OnSearchAlbumEvent, true);
        }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<AlbumsView>(), WorkspaceNameEnum.Albums.ToString());
        }

        private void OnSearchAlbumEvent(object parameter)
        {
            IAlbumSearchViewModel viewModel = container.Resolve<IAlbumSearchViewModel>();
            AlbumSearchDialog popup = new AlbumSearchDialog(viewModel);
            popup.ShowDialog();
        }
    }
}
