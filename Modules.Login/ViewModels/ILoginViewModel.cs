
using Common.Commands;
using Microsoft.Practices.Composite.Presentation.Commands;
namespace Modules.Login.ViewModels
{
    public interface ILoginViewModel
    {
        string UserName { get; set; }
        string Password { get; set; }
        bool IsBusy { get; }

        DelegateCommand<object> LoginCommand { get; }
        DelegateCommand<object> SetupDatabaseCommand { get; }
        DelegateCommand<KeyUpArgs> KeyUpCommand { get; }
        DelegateCommand<MouseDownArgs> RegisterNewUserCommand { get; }
    }
}
