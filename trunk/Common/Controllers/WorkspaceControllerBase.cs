
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
namespace Common.Controllers
{
    public abstract class WorkspaceControllerBase : ControllerBase
    {
        public WorkspaceControllerBase(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(container, regionManager, eventAggregator)
        {
            eventAggregator.GetEvent<ChangeWorkspaceEvent>().Subscribe(OnChangeWorkspaceViewEventHandler, true);
            InitViews();
        }

        protected abstract void InitViews();

        protected IRegion GetWorkspaceRegion()
        {
            return regionManager.Regions[RegionNames.WorkspaceRegion];
        }

        private void OnChangeWorkspaceViewEventHandler(WorkspaceNameEnum workspace)
        {
            DisplayWorkspace(workspace);
        }

        private void DisplayWorkspace(WorkspaceNameEnum workspace)
        {
            IRegion mainRegion = regionManager.Regions[RegionNames.WorkspaceRegion];

            object view = mainRegion.GetView(workspace.ToString());
            if (view != null && !IsActiveView(mainRegion, view))
            {
                mainRegion.Activate(view);

                eventAggregator.GetEvent<WorkspaceActivatedEvent>().Publish(workspace);
            }
        }

        private bool IsActiveView(IRegion region, object view)
        {
            return region.ActiveViews.Contains(view);
        }
    }
}
