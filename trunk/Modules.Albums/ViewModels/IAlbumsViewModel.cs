
using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Albums.ViewModels
{
    public interface IAlbumsViewModel : IFilterViewModel
    {
        ObservableCollection<Album> AlbumsCollection { get; }
        int AlbumsCount { get; }
        Album CurrentAlbum { get; set; }
        ILoadOptions LoadOptions { get; }
        bool IsBusy { get; }
        int VisibleAlbumsCount { get; set; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> EditAlbumCommand { get; }
        DelegateCommand<object> CreateAlbumCommand { get; }
        DelegateCommand<object> RemoveAlbumCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }
        DelegateCommand<object> BulkImportDataCommand { get; }
    }
}
