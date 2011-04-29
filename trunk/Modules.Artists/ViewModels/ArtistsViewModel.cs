
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Dialogs;
using Common.Dialogs.Helpers;
using Common.Entities.Pagination;
using Common.Enums;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Artists.Services;
using Modules.Artists.Views;
namespace Modules.Artists.ViewModels
{
    public class ArtistsViewModel : WasteableViewModelBase, IArtistsViewModel
    {
        public ArtistsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            eventAggregator.GetEvent<ReloadArtistsEvent>().Subscribe(OnReloadArtistsEvent, true);
            eventAggregator.GetEvent<AlbumLoadedEvent>().Subscribe(OnAlbumLoadedEvent, true);
            eventAggregator.GetEvent<CreateArtistEvent>().Subscribe(OnCreateArtistCommand, true);

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            EditArtistCommand = new DelegateCommand<object>(OnEditArtistCommand);
            RemoveArtistCommand = new DelegateCommand<object>(OnRemoveArtistCommand);
            CreateArtistCommand = new DelegateCommand<object>(OnCreateArtistCommand);
            EditAlbumCommand = new DelegateCommand<object>(OnEditAlbumCommand);
            RemoveAlbumCommand = new DelegateCommand<object>(OnRemoveAlbumCommand);
            CreateAlbumCommand = new DelegateCommand<object>(OnCreateAlbumCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);
            BulkImportDataCommand = new DelegateCommand<object>(OnBulkImportDataCommand);
        }

        public override IEnumerable<IField> InitializeFields()
        {
            IList<IField> fields = new List<IField>();

            fields.Add(new Field("Name", "Artist name"));
            fields.Add(new Field("PrivateMarks", "Private notes"));
            fields.Add(new Field("Biography", "Biography"));
            fields.Add(new Field("Tag", "Tag"));

            return fields;
        }

        public override void OnFilterValueChanged(IField selectedField, string selectedValue)
        {
            LoadOptions.FilterField = selectedField;
            LoadOptions.FilterValue = selectedValue;

            LoadArtists();
        }

        public override void OnShowWasteCommand(object parameter)
        {
            FilterByWaste(true);
        }

        public override void OnHideWasteCommand(object parameter)
        {
            FilterByWaste(false);
        }

        public override void OnMarkAsWasteCommand(object parameter)
        {
        }

        public override void OnUnMarkAsWasteCommand(object parameter)
        {
        }

        #region IArtistsViewModel Members

        public ObservableCollection<Artist> ArtistsCollection
        {
            get
            {
                return artistsCollection;
            }
            private set
            {
                artistsCollection = value;
                NotifyPropertyChanged(() => ArtistsCollection);
                NotifyPropertyChanged(() => ArtistsCount);
            }
        }

        public ObservableCollection<Album> ArtistAlbums
        {
            get
            {
                return artistAlbums;
            }
            private set
            {
                artistAlbums = value;
                NotifyPropertyChanged(() => ArtistAlbums);
            }
        }

        public int ArtistsCount
        {
            get
            {
                return artistsCount;
            }
            private set
            {
                if (value != artistsCount)
                {
                    artistsCount = value;
                    NotifyPropertyChanged(() => ArtistsCount);
                }
            }
        }

        public Artist CurrentArtist
        {
            get
            {
                return currentArtist;
            }
            set
            {
                currentArtist = value;
                NotifyPropertyChanged(() => CurrentArtist);

                LoadArtistAlbums();
            }
        }

        public Album CurrentAlbum
        {
            get
            {
                return currentAlbum;
            }
            set
            {
                currentAlbum = value;
                NotifyPropertyChanged(() => CurrentAlbum);
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
                loadOptions = value;
                NotifyPropertyChanged(() => LoadOptions);
            }
        }

        public bool IsLoadingAlbums
        {
            get
            {
                return isLoadingAlbums;
            }
            set
            {
                if (value != isLoadingAlbums)
                {
                    isLoadingAlbums = value;
                    NotifyPropertyChanged(() => IsLoadingAlbums);
                }
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        public DelegateCommand<object> CreateArtistCommand { get; private set; }

        public DelegateCommand<object> EditArtistCommand { get; private set; }

        public DelegateCommand<object> RemoveArtistCommand { get; private set; }

        public DelegateCommand<object> CreateAlbumCommand { get; private set; }

        public DelegateCommand<object> EditAlbumCommand { get; private set; }

        public DelegateCommand<object> RemoveAlbumCommand { get; private set; }

        public DelegateCommand<object> BulkImportDataCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnViewLoadedCommand(object parameter)
        {
            //User control is loaded multiple times when we switch between regions
            //We prevent additional data load when control is already been loaded
            if (InitialDataLoaded)
                return;
            else
            {
                InitialDataLoaded = true;

                LoadOptions = new LoadOptions();

                LoadOptions.FilterField = GetInitialField();
                LoadOptions.FilterValue = string.Empty;
                LoadOptions.FirstResult = 0;
                LoadOptions.MaxResults = 10;

                NotifyPropertyChanged(() => LoadOptions);

                LoadArtists();
            }
        }

        private void OnPageChangedCommand(PageChangedArgs parameter)
        {
            if (parameter == null)
                return;

            PageChangedEventArgs e = parameter.Settings;

            if (LoadOptions != null && e != null)
            {
                LoadOptions.FirstResult = e.PageIndex * e.ItemsPerPage;
                LoadArtists();
            }
        }

        private void OnCreateArtistCommand(object parameter)
        {
            Artist artist = new Artist();
            artist.NeedValidate = true;

            CreateOrEditArtist(artist, false);
        }

        private void OnRemoveArtistCommand(object parameter)
        {
            if (CurrentArtist == null)
            {
                Notify("Please select artist to be removed", NotificationType.Warning);
                return;
            }

            ConfirmDialog dialog = new ConfirmDialog()
            {
                HeaderText = "Artist remove confirmation",
                MessageText = "Do you really want to remove artist: " + CurrentArtist.Name + " ?"
            };

            if (dialog.ShowDialog() == true)
            {
                bool deleted = dataService.RemoveArtist(CurrentArtist);

                if (deleted)
                    Notify("Artist has been successfully removed", NotificationType.Success);
                else
                    Notify("Can't remove selected artist. See log for details", NotificationType.Error);

                LoadArtists();
            }
        }

        private void OnEditArtistCommand(object parameter)
        {
            if (CurrentArtist == null)
                Notify("You must select artist to edit it", NotificationType.Info);
            else
                EditArtistImpl();
        }

        private void OnCreateAlbumCommand(object parameter)
        {
            if (CurrentArtist != null)
            {
                eventAggregator.GetEvent<CreateAlbumEvent>().Publish(CurrentArtist);

                LoadArtistAlbums();
            }
            else
            {
                Notify("Please select artist first", NotificationType.Info);
            }
        }

        private void OnEditAlbumCommand(object parameter)
        {
            if (CurrentAlbum == null)
                return;

            IsBusy = true; // albums module will raise AlbumLoadedEvent and we clear busy status

            eventAggregator.GetEvent<EditAlbumEvent>().Publish(CurrentAlbum.ID);
        }

        private void OnRemoveAlbumCommand(object parameter)
        {
            if (CurrentAlbum == null)
                return;

            eventAggregator.GetEvent<RemoveAlbumEvent>().Publish(CurrentAlbum.ID);
        }

        private void OnBulkImportDataCommand(object parameter)
        {
            eventAggregator.GetEvent<ImportDataEvent>().Publish(null);
        }

        private void EditArtistImpl()
        {
            if (CurrentArtist == null)
                return;

            IsBusy = true;

            Task<Artist> loadArtistTask = Task.Factory.StartNew<Artist>(() =>
                {
                    return dataService.GetArtist(CurrentArtist.ID);
                }, TaskScheduler.Default);

            Task finishedTask = loadArtistTask.ContinueWith((t) =>
                {
                    IsBusy = false;

                    Artist artist = t.Result;

                    if (artist != null)
                    {
                        CreateOrEditArtist(artist, true);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void OnAlbumLoadedEvent(int albumID)
        {
            IsBusy = false;
        }

        private void OnReloadArtistsEvent(object parameter)
        {
            LoadArtists();
        }

        private void LoadArtists()
        {
            IPagedList<Artist> artists = dataService.GetArtists(LoadOptions);
            ArtistsCount = artists.TotalItems;
            ArtistsCollection = new ObservableCollection<Artist>(artists);
        }

        private void CreateOrEditArtist(Artist artist, bool isEditMode)
        {
            IArtistEditViewModel viewModel = container.Resolve<IArtistEditViewModel>();
            viewModel.IsEditMode = isEditMode;
            viewModel.Artist = artist;

            artist.NeedValidate = true;


            var dialog = new CommonDialog()
            {
                DialogContent = new ArtistEditView(viewModel),
                HeaderText = HeaderTextHelper.CreateHeaderText(typeof(Artist), isEditMode)
            };
            dialog.ShowDialog();
        }

        private void LoadArtistAlbums()
        {
            if (CurrentArtist == null)
                return;

            IsLoadingAlbums = true;

            Task<IList<Album>> loadArtistAlbumsTask = Task.Factory.StartNew<IList<Album>>(() =>
                {
                    return dataService.GetAlbumsByArtistID(CurrentArtist.ID);
                }, TaskScheduler.Default);

            Task finishTask = loadArtistAlbumsTask.ContinueWith((t) =>
                {
                    IsLoadingAlbums = false;
                    ArtistAlbums = new ObservableCollection<Album>(t.Result);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FilterByWaste(bool showWaste)
        {
            IsWasteVisible = showWaste;
            LoadOptions.IncludeWaste = showWaste;

            LoadArtists();
        }

        #endregion

        #region Private fields

        private IDataService dataService;

        private ObservableCollection<Artist> artistsCollection;
        private ObservableCollection<Album> artistAlbums;
        private int artistsCount;
        private Artist currentArtist;
        private Album currentAlbum;
        private ILoadOptions loadOptions;

        private bool isLoadingAlbums;

        #endregion
    }
}
