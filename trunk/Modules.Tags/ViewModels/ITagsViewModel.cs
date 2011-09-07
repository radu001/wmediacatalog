
using Microsoft.Practices.Prism.Commands;
namespace Modules.Tags.ViewModels
{
    public interface ITagsViewModel
    {
        DelegateCommand<object> ViewLoadedCommand { get; }
    }
}
