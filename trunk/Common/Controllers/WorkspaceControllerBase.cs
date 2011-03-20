
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
namespace Common.Controllers
{
    public abstract class WorkspaceControllerBase
    {
        public WorkspaceControllerBase(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

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

        protected readonly IUnityContainer container;
        protected readonly IEventAggregator eventAggregator;
        protected readonly IRegionManager regionManager;
    }
}
