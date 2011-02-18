
using Common.Controllers;
using Common.Enums;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Artists.Views;
namespace Modules.Artists.Controllers
{
    public class ArtistsController : WorkspaceControllerBase
    {
        public ArtistsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator) { }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<ArtistsView>(), WorkspaceNameEnum.Artists.ToString());
        }
    }
}
