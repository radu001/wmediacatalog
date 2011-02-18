using Common.Commands;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Modules.WorkspaceSelector.ViewModels
{
    public interface IWorkspaceSelectorViewModel
    {
        DelegateCommand<SelectionChangedArgs> WorkspaceChangeCommand { get; }
    }
}
