
using Common.Controllers;
using Common.Enums;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Listenings.Views;
namespace Modules.Listenings.Controllers
{
    public class ListeningsController : WorkspaceControllerBase
    {
        public ListeningsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator) { }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<ListeningsView>(), WorkspaceNameEnum.Listenings.ToString());
        }
    }
}
