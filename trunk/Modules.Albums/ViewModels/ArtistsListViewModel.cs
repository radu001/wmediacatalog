using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessObjects;
using Common.Commands;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Albums.Events;
using Modules.Albums.Services;

namespace Modules.Albums.ViewModels
{
    public class ArtistsListViewModel : ViewModelBase, IArtistListViewModel, IEventSubscriber
    {
        public ArtistsListViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            SubscribeEvents();

            FilterField = new Field("Name", "Artist name");

            HideShowArtistsListCommand = new DelegateCommand<object>(OnHideShowArtistsListCommand);
            AttachArtistsCommand = new DelegateCommand<object>(OnAttachArtistsCommand);
            AttachArtistsKeyboardCommand = new DelegateCommand<object>(OnAttachArtistsKeyboardCommand);
            DetachArtistsCommand = new DelegateCommand<object>(OnDetachArtistsCommand);
            CreateArtistCommand = new DelegateCommand<object>(OnCreateArtistCommand);
            SelectedArtistsChangedCommand = new DelegateCommand<MultiSelectionChangedArgs>(OnSelectedArtistsChangedCommand);
            DragArtistCommand = new DelegateCommand<MouseMoveArgs>(OnDragArtistsCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);

            ArtistsFilterValue = String.Empty;
        }

        #region IArtistListViewModel Members

        public IList<Artist> Artists
        {
            get
            {
                return artists;
            }
            private set
            {
                artists = value;
                NotifyPropertyChanged(() => Artists);
            }
        }

        public IList<Artist> SelectedArtists
        {
            get
            {
                return selectedArtists;
            }
            private set
            {
                selectedArtists = value;
                NotifyPropertyChanged(() => SelectedArtists);
            }
        }

        public ILoadOptions LoadOptions
        {
            get
            {
                return loadOptions;
            }
            private set
            {
                if (value != loadOptions)
                {
                    loadOptions = value;
                    NotifyPropertyChanged(() => LoadOptions);
                }
            }
        }

        public IField FilterField
        {
            get
            {
                return filterField;
            }
            private set
            {
                filterField = value;
                NotifyPropertyChanged(() => FilterField);
            }
        }

        public string ArtistsFilterValue
        {
            get
            {
                return artistsFilterValue;
            }
            set
            {
                if (value != artistsFilterValue)
                {
                    artistsFilterValue = value;
                    NotifyPropertyChanged(() => ArtistsFilterValue);

                    LoadArtists();
                }
            }
        }

        public int TotalArtistsCount
        {
            get
            {
                return totalArtistsCount;
            }
            private set
            {
                if (value != totalArtistsCount)
                {
                    totalArtistsCount = value;
                    NotifyPropertyChanged(() => TotalArtistsCount);
                }
            }
        }

        public bool IsArtistsListVisible
        {
            get
            {
                return isArtistsListVisible;
            }
            private set
            {
                if (value != isArtistsListVisible)
                {
                    isArtistsListVisible = value;
                    NotifyPropertyChanged(() => IsArtistsListVisible);
                }
            }
        }

        public DelegateCommand<object> HideShowArtistsListCommand { get; private set; }

        public DelegateCommand<object> AttachArtistsCommand { get; private set; }

        public DelegateCommand<object> AttachArtistsKeyboardCommand { get; private set; }

        public DelegateCommand<object> DetachArtistsCommand { get; private set; }

        public DelegateCommand<object> CreateArtistCommand { get; private set; }

        public DelegateCommand<MultiSelectionChangedArgs> SelectedArtistsChangedCommand { get; private set; }

        public DelegateCommand<MouseMoveArgs> DragArtistCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        #endregion

        #region IEventSubscriber Members

        public void SubscribeEvents()
        {
            eventAggregator.GetEvent<ReloadArtistsEvent>().Subscribe(OnArtistCreatedEvent, true);
        }

        public void UnsubscribeEvents()
        {
            eventAggregator.GetEvent<ReloadArtistsEvent>().Unsubscribe(OnArtistCreatedEvent);
        }

        #endregion

        #region Private methods

        private void OnPageChangedCommand(PageChangedArgs parameter)
        {
            if (LoadOptions == null || parameter == null)
                return;

            LoadOptions.FirstResult = parameter.Settings.PageIndex * parameter.Settings.ItemsPerPage;

            LoadArtistsImpl();
        }

        private void LoadArtists()
        {
            if (LoadOptions == null)
                LoadOptions = new LoadOptions();

            LoadOptions.FilterField = this.FilterField;
            LoadOptions.FilterValue = ArtistsFilterValue;
            LoadOptions.FirstResult = 0;
            LoadOptions.MaxResults = 8;

            LoadArtistsImpl();

            NotifyPropertyChanged(() => LoadOptions);
        }

        private void LoadArtistsImpl()
        {
            IPagedList<Artist> artistsList = dataService.GetArtists(LoadOptions);
            TotalArtistsCount = artistsList.TotalItems;
            Artists = artistsList;
        }

        private void OnHideShowArtistsListCommand(object parameter)
        {
            IsArtistsListVisible = !IsArtistsListVisible;
        }

        private void OnAttachArtistsCommand(object parameter)
        {
            if (SelectedArtists != null && SelectedArtists.Count > 0)
            {
                RaiseAttachArtists(SelectedArtists);
            }
        }

        private void OnAttachArtistsKeyboardCommand(object parameter)
        {
            /*
             * Currently Artists filtered collection is loaded synchronously. This means that when we press Enter key
             * Artists collection is already loaded. Please take into account this behavour in case of any changes 
             */

            KeyEventArgs args = parameter as KeyEventArgs;
            if (args != null && args.Key == Key.Enter)
            {
                var matchingArtist = Artists.Where(a => a.Name == ArtistsFilterValue).FirstOrDefault();
                if (matchingArtist == null)
                {
                    RaiseCreateArtist(ArtistsFilterValue);
                }
                else
                {
                    SelectedArtists = new List<Artist>() { matchingArtist };
                    RaiseAttachArtists(SelectedArtists);
                }
            }
        }

        private void OnDetachArtistsCommand(object parameter)
        {
            eventAggregator.GetEvent<DetachArtistsEvent>().Publish(null);
        }

        private void OnCreateArtistCommand(object parameter)
        {
            RaiseCreateArtist(ArtistsFilterValue);
        }

        private void OnSelectedArtistsChangedCommand(MultiSelectionChangedArgs parameter)
        {
            if (parameter == null)
                return;

            SelectedArtists = parameter.GetSelectedValues<Artist>();
        }

        private void OnDragArtistsCommand(MouseMoveArgs parameter)
        {
            if (parameter == null)
                return;

            ListView dragSource = parameter.Sender as ListView;
            if (dragSource == null)
                return;

            if (SelectedArtists == null)
                return;
            if (SelectedArtists.Count < 1)
                return;

            if (parameter.Settings.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dataObject = new DataObject(typeof(Artist), SelectedArtists[0]);

                DragDrop.DoDragDrop(dragSource, dataObject, DragDropEffects.Copy);

                // allow scroll inside listView
                dragSource.SelectedItems.Clear();
                SelectedArtists = null;
            }
        }

        private void OnArtistCreatedEvent(object parameter)
        {
            LoadArtists();
        }

        private void RaiseCreateArtist(string artistName)
        {
            eventAggregator.GetEvent<CreateArtistEvent>().Publish(artistName);
        }

        private void RaiseAttachArtists(IList<Artist> artists)
        {
            if (artists == null)
                throw new NullReferenceException("Illegal null-reference artists");

            eventAggregator.GetEvent<AttachArtistsEvent>().Publish(artists);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private IList<Artist> artists;
        private IList<Artist> selectedArtists;

        private ILoadOptions loadOptions;
        private IField filterField;
        private string artistsFilterValue;
        private int totalArtistsCount;

        private bool isArtistsListVisible;

        #endregion
    }
}
