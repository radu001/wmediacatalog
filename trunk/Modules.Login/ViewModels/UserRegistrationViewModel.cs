using System;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Modules.Login.Services;

namespace Modules.Login.ViewModels
{
    public class UserRegistrationViewModel : DialogViewModelBase, IUserRegistrationViewModel
    {
        public UserRegistrationViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            RegisterNewUserCommand = new DelegateCommand<object>(OnSuccessCommand);
        }

        public override void OnSuccessCommand(object parameter)
        {
            if (!ValidateBeforeSave(parameter))
            {
                Notify("Please fill all required fields", NotificationType.Warning);
                return;
            }

            if (dataService.UserExists(UserName))
            {
                Notify("Registration failed. User with given name already exists. Name=" + UserName, NotificationType.Error);
                return;
            }

            IsBusy = true;

            Task<bool> createUserTask = Task.Factory.StartNew<bool>(() =>
            {
                PasswordHelper passwordHelper = new PasswordHelper();
                string hashedPassword = passwordHelper.CreateBase64Hash(Password);

                User newUser = new User()
                {
                    UserName = this.UserName,
                    Password = hashedPassword
                };

                return dataService.SaveUser(newUser);
            }, TaskScheduler.Default);

            Task finishTask = createUserTask.ContinueWith((t) =>
            {
                IsBusy = false;

                bool success = t.Result;

                if (success)
                    Notify("New user has been successfully created.", NotificationType.Success);
                else
                    Notify("Can't create new user (internal error). Please see log for details.", NotificationType.Error);
                DialogResult = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        #region IUserRegistrationViewModel Members

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exception("User name mustn't be empty");

                userName = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exception("Password mustn't be empty");

                password = value;
            }
        }

        public DelegateCommand<object> RegisterNewUserCommand { get; private set; }

        #endregion

        #region Private methods

        private bool ValidateBeforeSave(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            return validator.Validate(parameter);
        }

        #endregion

        #region Private fields

        IDataService dataService;
        private string userName;
        private string password;

        #endregion
    }
}
