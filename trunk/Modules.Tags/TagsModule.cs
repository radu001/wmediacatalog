
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Tags.Services;
using Modules.Tags.ViewModels;
namespace Modules.Tags
{
    public class TagsModule : IModule
    {
        public TagsModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<ITagEditViewModel, TagEditViewModel>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
    }
}
