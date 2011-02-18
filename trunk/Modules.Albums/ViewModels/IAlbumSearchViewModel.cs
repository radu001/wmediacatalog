using Common.ViewModels;
using Microsoft.Practices.Composite.Presentation.Commands;
using Common.Controls.Controls;
using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Entities.Pagination;

namespace Modules.Albums.ViewModels
{
    public interface IAlbumSearchViewModel : IDialogViewModel, IFilterViewModel
    {
        ObservableCollection<Album> AlbumsCollection { get; }
        Album CurrentAlbum { get; set; }
        ILoadOptions LoadOptions { get; }
        int AlbumsCount { get; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> SelectAlbumCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }

    }
}
