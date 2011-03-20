
using System.Collections.Generic;
using BusinessObjects;
using Common.Commands;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Albums.ViewModels
{
    public interface ITracksListViewModel
    {
        Album Album { get; set; }
        Track CurrentTrack { get; set; }
        IList<Track> SelectedTracks { get; }

        DelegateCommand<object> AddTrackCommand { get; }
        DelegateCommand<object> RemoveTracksCommand { get; }
        DelegateCommand<MultiSelectionChangedArgs> SelectedTracksChangedCommand { get; }
        DelegateCommand<object> MoveTracksDownCommand { get; }
        DelegateCommand<object> MoveTracksUpCommand { get; }
    }
}
