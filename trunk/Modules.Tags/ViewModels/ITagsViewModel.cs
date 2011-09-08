
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using TagCloudLib;
namespace Modules.Tags.ViewModels
{
    public interface ITagsViewModel
    {
        ObservableCollection<ITag> Tags { get; }
        ObservableCollection<ITag> SelectedTags { get; }
        ITag AllTagsSelectedItem { get; set; }
        ITag SelectedTagsSelectedItem { get; set; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> SelectedTagsDropCommand { get; }
        DelegateCommand<object> AllTagsDropCommand { get; }
        DelegateCommand<object> SelectedTagsDragCommand { get; }
        DelegateCommand<object> AllTagsDragCommand { get; }
    }
}
