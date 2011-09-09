
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using TagCloudLib;
using BusinessObjects.Artificial;
using Common.Entities.Pagination;
using Common.Controls.Controls;
namespace Modules.Tags.ViewModels
{
    public interface ITagsViewModel
    {
        ObservableCollection<ITag> Tags { get; }
        ObservableCollection<ITag> SelectedTags { get; }
        IPagedList<TaggedObject> TaggedObjects { get; }
        ITag AllTagsSelectedItem { get; set; }
        ITag SelectedTagsSelectedItem { get; set; }
        ILoadOptions LoadOptions { get; }
        int TaggedObjectsCount { get; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> SelectedTagsDropCommand { get; }
        DelegateCommand<object> AllTagsDropCommand { get; }
        DelegateCommand<object> SelectedTagsDragCommand { get; }
        DelegateCommand<object> AllTagsDragCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }
        DelegateCommand<object> TagDoubleClickedCommand { get; }
    }
}
