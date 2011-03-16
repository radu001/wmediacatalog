using Common.Controllers;
using Common.Enums;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Import.Views;

namespace Modules.Import.Controllers
{
    public class ImportController : WorkspaceControllerBase
    {
        public ImportController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator) { }

        protected override void InitViews()
        {
            IRegion workspaceRegion = GetWorkspaceRegion();
            workspaceRegion.Add(container.Resolve<ImportHolderView>(), WorkspaceNameEnum.Import.ToString());
        }
    }
}
