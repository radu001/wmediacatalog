
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Login.ViewModels
{
    public interface IUserRegistrationViewModel : IDialogViewModel
    {
        string UserName { get; set; }
        string Password { get; set; }

        DelegateCommand<object> RegisterNewUserCommand { get; }
    }
}
