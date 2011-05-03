using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;

namespace Modules.Albums.ViewModels
{
    public interface IAlbumSearchViewModel : IDialogViewModel, IFilterViewModel, IEventSubscriber
    {
        ObservableCollection<Album> AlbumsCollection { get; }
        Album CurrentAlbum { get; set; }
        ILoadOptions LoadOptions { get; }
        int AlbumsCount { get; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> SelectAlbumCommand { get; }
        DelegateCommand<object> CreateAlbumCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }

    }
}
