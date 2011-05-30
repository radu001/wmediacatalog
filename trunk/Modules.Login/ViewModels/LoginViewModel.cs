
using System.Threading.Tasks;
using System.Windows.Controls;
using BusinessObjects;
using Common.Commands;
using Common.Dialogs;
using Common.Dialogs.Helpers;
using Common.Entities;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Login.Services;
using Modules.Login.Views;
namespace Modules.Login.ViewModels
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        public LoginViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService) :
            base(container, eventAggregator)
        {
            this.dataService = dataService;

            LoginCommand = new DelegateCommand<object>(OnLoginCommand);
            SetupDatabaseCommand = new DelegateCommand<object>(OnSetupDatabaseCommand);
            KeyUpCommand = new DelegateCommand<KeyUpArgs>(OnKeyUpCommand);
            RegisterNewUserCommand = new DelegateCommand<MouseDownArgs>(OnRegisterNewUserCommand);
        }

        #region ILoginViewModel

        public string UserName { get; set; }

        public string Password { get; set; }

        public DelegateCommand<object> LoginCommand { get; private set; }
        public DelegateCommand<object> SetupDatabaseCommand { get; private set; }
        public DelegateCommand<KeyUpArgs> KeyUpCommand { get; private set; }
        public DelegateCommand<MouseDownArgs> RegisterNewUserCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnLoginCommand(object parameter)
        {
            SetupDatabaseCommand.Execute(null);
        }

        private void OnSetupDatabaseCommand(object parameter)
        {
            IsBusy = true;

            Task<object> validateDatabaseTask = Task.Factory.StartNew<object>(() =>
            {
                return dataService.ValidateConnection();
            }, TaskScheduler.Default);

            Task finishTask = validateDatabaseTask.ContinueWith((t) =>
            {
                IsBusy = false;

                if (t.Result == null) // database is ok
                {
                    PerformLogin();
                }
                else
                {
                    NotifyDatabaseUnavaliable();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void PerformLogin()
        {
            AuthorizationInfo info = new AuthorizationInfo(UserName, Password);

            Task<User> validateUserTask = Task.Factory.StartNew<User>(() =>
            {
                PasswordHelper passwordHelper = new PasswordHelper();
                string hashedPassword = passwordHelper.CreateBase64Hash(Password);

                return dataService.ValidateCredentials(UserName, hashedPassword);
            }, TaskScheduler.Default);

            Task finishTask = validateUserTask.ContinueWith((t) =>
            {
                IsBusy = false;
                User user = t.Result;

                if (user != null)
                {
                    dataService.UserLoggedIn(user);
                    eventAggregator.GetEvent<LoginSucceededEvent>().Publish(info);
                }
                else
                {
                    eventAggregator.GetEvent<LoginFailedEvent>().Publish(info);
                    Notify("Authorization failed. Invalid user name and(or) password.", NotificationType.Error);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void NotifyDatabaseUnavaliable()
        {
            Notify("Can't connect to database. Please validate configuration settings", NotificationType.Warning);

            eventAggregator.GetEvent<SetupDatabaseEvent>().Publish(null);
        }

        private void OnKeyUpCommand(KeyUpArgs parameter)
        {
            PasswordBox passwordBox = parameter.Sender as PasswordBox;

            if (passwordBox != null)
            {
                Password = passwordBox.Password;
            }
        }

        private void OnRegisterNewUserCommand(MouseDownArgs parameter)
        {
            var viewModel = container.Resolve<IUserRegistrationViewModel>();

            var dialog = new CommonDialog()
            {
                DialogContent = new UserRegistrationView(viewModel),
                HeaderText = HeaderTextHelper.CreateHeaderText(typeof(User), false)
            };
            dialog.ShowDialog();
        }

        #endregion

        #region Private fields

        private IDataService dataService;

        #endregion
    }
}
