using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Events;
using Common.Enums;
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
