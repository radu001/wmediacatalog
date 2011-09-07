
using Microsoft.Practices.Prism.Commands;
using BusinessObjects;
using System.Collections.ObjectModel;
namespace Modules.Tags.ViewModels
{
    public interface ITagsViewModel
    {
        ObservableCollection<Tag> Tags { get; }

        DelegateCommand<object> ViewLoadedCommand { get; }
    }
}
