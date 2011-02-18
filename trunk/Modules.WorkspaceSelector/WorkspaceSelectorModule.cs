using Common;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.WorkspaceSelector.Services;
using Modules.WorkspaceSelector.ViewModels;
using Modules.WorkspaceSelector.Views;

namespace Modules.WorkspaceSelector
{
    public class WorkspaceSelectorModule : IModule
    {
        public WorkspaceSelectorModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IWorkspaceSelectorViewModel, WorkspaceSelectorViewModel>();
            regionManager.RegisterViewWithRegion(RegionNames.WorkspaceSelectorRegion, typeof(WorkspaceSelectorView));
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
    }
}
