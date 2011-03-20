
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BusinessObjects;
using Common.Commands;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Albums.Utils;

namespace Modules.Albums.ViewModels
{
    public class TracksListViewModel : ViewModelBase, ITracksListViewModel
    {
        public TracksListViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            AddTrackCommand = new DelegateCommand<object>(OnAddTrackCommand);
            RemoveTracksCommand = new DelegateCommand<object>(OnRemoveTracksCommand);
            SelectedTracksChangedCommand = new DelegateCommand<MultiSelectionChangedArgs>(OnSelectedTracksChangedCommand);
            MoveTracksDownCommand = new DelegateCommand<object>(OnMoveTracksDown);
            MoveTracksUpCommand = new DelegateCommand<object>(OnMoveTracksUp);
        }

        #region ITracksListViewModel Members

        public Album Album
        {
            get
            {
                return album;
            }
            set
            {
                album = value;
                NotifyPropertyChanged(() => Album);
            }
        }

        public Track CurrentTrack
        {
            get
            {
                return currentTrack;
            }
            set
            {
                currentTrack = value;
                NotifyPropertyChanged(() => CurrentTrack);
            }
        }

        public IList<Track> SelectedTracks
        {
            get
            {
                return selectedTracks;
            }
            set
            {
                if (value != null)
                {
                    selectedTracks = value;
                    NotifyPropertyChanged(() => SelectedTracks);
                }
            }
        }

        public DelegateCommand<object> AddTrackCommand { get; private set; }

        public DelegateCommand<object> RemoveTracksCommand { get; private set; }

        public DelegateCommand<MultiSelectionChangedArgs> SelectedTracksChangedCommand { get; private set; }

        public DelegateCommand<object> MoveTracksDownCommand { get; private set; }

        public DelegateCommand<object> MoveTracksUpCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnAddTrackCommand(object parameter)
        {
            if (Album == null)
                return;

            int maxIndex = 0;

            if (Album.Tracks.Count > 0)
                maxIndex = Album.Tracks.Max(t => t.Index);

            Track track = new Track()
            {
                Index = maxIndex + 1
            };
            Album.Tracks.Add(track);
            CurrentTrack = track;

        }

        private void OnRemoveTracksCommand(object parameter)
        {
            if (Album == null || SelectedTracks == null)
                return;

            foreach (Track t in SelectedTracks)
            {
                Album.Tracks.Remove(t);
            }

            FixIndices();
        }

        private void OnSelectedTracksChangedCommand(MultiSelectionChangedArgs parameter)
        {
            if (parameter == null)
                return;

            SelectedTracks = parameter.GetSelectedValues<Track>();
        }

        private void OnMoveTracksDown(object parameter)
        {
            if (SelectedTracks == null)
                return;

            TracksHelper helper = new TracksHelper(Album.Tracks, SelectedTracks);
            helper.MoveDown();
            helper.SelectTracks(GetDataGrid(parameter));

            FixIndices();
        }

        private void OnMoveTracksUp(object parameter)
        {
            if (SelectedTracks == null)
                return;

            TracksHelper helper = new TracksHelper(Album.Tracks, SelectedTracks);
            helper.MoveUp();
            helper.SelectTracks(GetDataGrid(parameter));

            FixIndices();
        }

        /// <summary>
        /// Fixes all indices in Album.Tracks collection so they form ascending sequence from 1 to N 
        /// N - total tracks count
        /// </summary>
        private void FixIndices()
        {
            if (Album == null)
                return;

            if (Album.Tracks == null)
                return;

            int totalTracks = Album.Tracks.Count;
            int currentTrackIndex = 1;

            while (currentTrackIndex <= totalTracks)
            {
                Track track = Album.Tracks[currentTrackIndex - 1];
                track.Index = currentTrackIndex;
                ++currentTrackIndex;
            }
        }

        private DataGrid GetDataGrid(object parameter)
        {
            if (parameter == null)
                return null;

            if (parameter is DataGrid)
                return parameter as DataGrid;

            if (parameter is MenuItemClickArgs)
            {
                MenuItemClickArgs args = parameter as MenuItemClickArgs;
                return args.MenuOwner as DataGrid;
            }

            return null;
        }

        #endregion

        #region Private fields

        private Album album;
        private Track currentTrack;
        private IList<Track> selectedTracks;

        #endregion


    }
}
