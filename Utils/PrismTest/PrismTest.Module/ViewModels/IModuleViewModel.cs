
using Microsoft.Practices.Prism.Commands;
namespace PrismTest.Module.ViewModels
{
    public interface IModuleViewModel
    {
        DelegateCommand<object> StartWizardCommand { get; }
    }
}
