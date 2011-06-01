
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.Tags.Controllers;
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
            container.RegisterType<ITagsViewModel, TagsViewModel>();
            container.RegisterType<ITagEditViewModel, TagEditViewModel>();

            controller = container.Resolve<TagsController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private TagsController controller;
    }
}
