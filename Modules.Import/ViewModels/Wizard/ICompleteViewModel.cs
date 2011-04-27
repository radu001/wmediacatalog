using Microsoft.Practices.Prism.Commands;

namespace Modules.Import.ViewModels.Wizard
{
    public interface ICompleteViewModel
    {
        DelegateCommand<object> CompleteWizardCommand { get; }
    }
}
