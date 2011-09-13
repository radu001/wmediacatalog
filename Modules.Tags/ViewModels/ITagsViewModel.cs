
using System.Collections.ObjectModel;
using BusinessObjects.Artificial;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Microsoft.Practices.Prism.Commands;
using TagCloudLib;
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
        string EntityNameFilter { get; set; }
        bool ShowAlbumsFilter { get; set; }
        bool ShowArtistsFilter { get; set; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> SelectedTagsDropCommand { get; }
        DelegateCommand<object> AllTagsDropCommand { get; }
        DelegateCommand<object> SelectedTagsDragCommand { get; }
        DelegateCommand<object> AllTagsDragCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }
        DelegateCommand<object> TagDoubleClickedCommand { get; }
        DelegateCommand<object> MoveTagUpCommand { get; }
        DelegateCommand<object> MoveTagDownCommand { get; }
        DelegateCommand<object> ClearSelectedTagsCommand { get; }
    }
}
