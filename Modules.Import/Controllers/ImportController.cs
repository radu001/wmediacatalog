using Common.Controllers;
using Common.Enums;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
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
            workspaceRegion.Add(container.Resolve<ImportView>(), WorkspaceNameEnum.Import.ToString());
        }
    }
}
