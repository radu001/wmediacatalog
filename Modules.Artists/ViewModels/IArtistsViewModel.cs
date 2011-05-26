
using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Artists.ViewModels
{
    public interface IArtistsViewModel : IFilterViewModel
    {
        ObservableCollection<Artist> ArtistsCollection { get; }
        ObservableCollection<Album> ArtistAlbums { get; }
        int ArtistsCount { get; }
        Artist CurrentArtist { get; set; }
        Album CurrentAlbum { get; set; }
        ILoadOptions LoadOptions { get; }
        bool IsBusy { get; }
        bool IsLoadingAlbums { get; }
        int VisibleArtistsCount { get; set; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }

        DelegateCommand<string> CreateArtistCommand { get; }
        DelegateCommand<object> EditArtistCommand { get; }
        DelegateCommand<object> RemoveArtistCommand { get; }

        DelegateCommand<object> CreateAlbumCommand { get; }
        DelegateCommand<object> EditAlbumCommand { get; }
        DelegateCommand<object> RemoveAlbumCommand { get; }

        DelegateCommand<object> BulkImportDataCommand { get; }
    }
}
