
using Microsoft.Practices.Prism.Commands;
namespace PrismTest.Module.ViewModels
{
    public interface IInitialViewModel
    {
        DelegateCommand<object> CompleteStepCommand { get; }
    }
}
