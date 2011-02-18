using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Modules.Login.Controllers;
using Modules.Login.Services;
using Modules.Login.ViewModels;
using Modules.Login.Views;

namespace Modules.Login
{
    public class LoginModule : IModule
    {
        public LoginModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<ILoginViewModel, LoginViewModel>();
            container.RegisterType<IUserRegistrationViewModel, UserRegistrationViewModel>();

            container.RegisterType<IUserRegistrationView, UserRegistrationView>();

            loginController = container.Resolve<LoginController>();
        }

        #endregion

        private IRegionManager regionManager;
        private IUnityContainer container;
        private LoginController loginController;
    }
}
