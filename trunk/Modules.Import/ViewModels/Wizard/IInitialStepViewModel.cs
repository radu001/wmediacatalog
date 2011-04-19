
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public interface IInitialStepViewModel : IWizardViewModel
    {
        string Message { get; }
    }
}
