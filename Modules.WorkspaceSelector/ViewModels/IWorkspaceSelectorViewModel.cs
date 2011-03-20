using Common.Commands;
using Microsoft.Practices.Prism.Commands;

namespace Modules.WorkspaceSelector.ViewModels
{
    public interface IWorkspaceSelectorViewModel
    {
        DelegateCommand<SelectionChangedArgs> WorkspaceChangeCommand { get; }
    }
}
