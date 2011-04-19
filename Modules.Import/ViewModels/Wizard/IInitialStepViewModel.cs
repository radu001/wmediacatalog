using Microsoft.Practices.Prism.Commands;

namespace Modules.Import.ViewModels.Wizard
{
    public interface IInitialStepViewModel
    {
        string Message { get; }

        DelegateCommand<object> ContinueCommand { get; }
    }
}
