using Common.Controllers;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Listenings.Controllers;
using Modules.Listenings.Services;
using Modules.Listenings.ViewModels;

namespace Modules.Listenings
{
    public class ListeningsModule : IModule
    {
        public ListeningsModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IPlaceViewModel, PlaceViewModel>();
            container.RegisterType<IMoodViewModel, MoodViewModel>();
            container.RegisterType<IListeningsViewModel, ListeningsViewModel>();
            container.RegisterType<IListeningEditViewModel, ListeningEditViewModel>();

            controller = container.Resolve<ListeningsController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private WorkspaceControllerBase controller;
    }
}
