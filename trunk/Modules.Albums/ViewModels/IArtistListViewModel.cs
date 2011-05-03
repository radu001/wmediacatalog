
using System.Collections.Generic;
using BusinessObjects;
using Common.Commands;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Albums.ViewModels
{
    public interface IArtistListViewModel : IEventSubscriber
    {
        IList<Artist> Artists { get; }
        IList<Artist> SelectedArtists { get; }
        ILoadOptions LoadOptions { get; }
        IField FilterField { get; }
        string ArtistsFilterValue { get; set; }
        int TotalArtistsCount { get; }

        bool IsArtistsListVisible { get; }

        DelegateCommand<object> HideShowArtistsListCommand { get; }
        DelegateCommand<object> AttachArtistsCommand { get; }
        DelegateCommand<object> AttachArtistsKeyboardCommand { get; }
        DelegateCommand<object> DetachArtistsCommand { get; }
        DelegateCommand<object> CreateArtistCommand { get; }
        DelegateCommand<MultiSelectionChangedArgs> SelectedArtistsChangedCommand { get; }
        DelegateCommand<MouseMoveArgs> DragArtistCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }
    }
}
