
using Microsoft.Practices.Composite.Presentation.Commands;
using Common.ViewModels;
namespace Modules.Login.ViewModels
{
    public interface IUserRegistrationViewModel : IDialogViewModel
    {
        string UserName { get; set; }
        string Password { get; set; }

        DelegateCommand<object> RegisterNewUserCommand { get; }
    }
}
