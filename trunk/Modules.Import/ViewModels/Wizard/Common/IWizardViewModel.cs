
using Microsoft.Practices.Prism.Commands;
namespace Modules.Import.ViewModels.Wizard.Common
{
    public interface IWizardViewModel
    {
        DelegateCommand<object> ContinueCommand { get; }
    }
}
