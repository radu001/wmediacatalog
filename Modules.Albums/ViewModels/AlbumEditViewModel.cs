
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessObjects;
using Common.Commands;
using Common.Enums;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Albums.Events;
using Modules.Albums.Services;
using Modules.Tags.ViewModels;
using Modules.Tags.Views;
namespace Modules.Albums.ViewModels
{
    public class AlbumEditViewModel : DialogViewModelBase, IAlbumEditViewModel
    {
        public AlbumEditViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            eventAggregator.GetEvent<TagsChangedEvent>().Subscribe(OnTagsChangedEvent, true);
            eventAggregator.GetEvent<AttachGenresEvent>().Subscribe(OnAttachGenresEvent, true);
            eventAggregator.GetEvent<DetachGenresEvent>().Subscribe(OnDetachGenresEvent, true);
            eventAggregator.GetEvent<AttachArtistsEvent>().Subscribe(OnAttachArtistsEvent, true);
            eventAggregator.GetEvent<DetachArtistsEvent>().Subscribe(OnDetachArtistsEvent, true);

            FilterTagCommand = new AutoCompleteFilterPredicate<object>(FilterTag);
            AttachTagCommand = new DelegateCommand<object>(OnAttachTagCommand);
            AttachTagKeyboardCommand = new DelegateCommand<KeyDownArgs>(OnAttachTagKeyboardCommand);
            DetachTagCommand = new DelegateCommand<object>(OnDetachTagCommand);
            EditTagCommand = new DelegateCommand<MouseDoubleClickArgs>(OnEditTagCommand);

            DropGenreCommand = new DelegateCommand<DragArgs>(OnDropGenreCommand);
            SelectedGenresChangedCommand = new DelegateCommand<MultiSelectionChangedArgs>(OnSelectedGenresChangedCommand);
            DetachGenreCommand = new DelegateCommand<object>(OnDetachGenreCommand);

            DropArtistCommand = new DelegateCommand<DragArgs>(OnDropArtistCommand);
            SelectedArtistsChangedCommand = new DelegateCommand<MultiSelectionChangedArgs>(OnSelectedArtistsChangedCommand);
            DetachArtistCommand = new DelegateCommand<object>(OnDetachArtistCommand);

            LoadTags();

            GenresListViewModel = container.Resolve<IGenresListViewModel>();
            ArtistsListViewModel = container.Resolve<IArtistListViewModel>();
            TracksListViewModel = container.Resolve<ITracksListViewModel>();
        }

        #region IAlbumEditViewModel Members

        public IGenresListViewModel GenresListViewModel
        {
            get
            {
                return genresListViewModel;
            }
            private set
            {
                genresListViewModel = value;
                NotifyPropertyChanged(() => GenresListViewModel);
            }
        }

        public IArtistListViewModel ArtistsListViewModel
        {
            get
            {
                return artistsListViewModel;
            }
            set
            {
                artistsListViewModel = value;
                NotifyPropertyChanged(() => ArtistsListViewModel);
            }
        }

        public ITracksListViewModel TracksListViewModel
        {
            get
            {
                return trackListViewModel;
            }
            set
            {
                trackListViewModel = value;
                NotifyPropertyChanged(() => TracksListViewModel);
            }
        }

        public Album Album
        {
            get
            {
                return album;
            }
            set
            {
                album = value;
                TracksListViewModel.Album = value;
                NotifyPropertyChanged(() => Album);
            }
        }

        public IList<Tag> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
                NotifyPropertyChanged(() => Tags);
            }
        }

        public Tag SelectedTag
        {
            get
            {
                return selectedTag;
            }
            set
            {
                selectedTag = value;
                NotifyPropertyChanged(() => SelectedTag);
            }
        }

        public string NewTagName
        {
            get
            {
                return newTagName;
            }
            set
            {
                newTagName = value;
                NotifyPropertyChanged(() => NewTagName);
            }
        }

        public IList<Genre> SelectedGenres
        {
            get
            {
                return selectedGenres;
            }
            private set
            {
                selectedGenres = value;
                NotifyPropertyChanged(() => SelectedGenres);
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

        public AutoCompleteFilterPredicate<object> FilterTagCommand { get; private set; }

        public DelegateCommand<object> AttachTagCommand { get; private set; }

        public DelegateCommand<KeyDownArgs> AttachTagKeyboardCommand { get; private set; }

        public DelegateCommand<object> DetachTagCommand { get; private set; }

        public DelegateCommand<MouseDoubleClickArgs> EditTagCommand { get; private set; }

        public DelegateCommand<DragArgs> DropGenreCommand { get; private set; }

        public DelegateCommand<MultiSelectionChangedArgs> SelectedGenresChangedCommand { get; private set; }

        public DelegateCommand<object> DetachGenreCommand { get; private set; }

        public DelegateCommand<DragArgs> DropArtistCommand { get; private set; }

        public DelegateCommand<MultiSelectionChangedArgs> SelectedArtistsChangedCommand { get; private set; }

        public DelegateCommand<object> DetachArtistCommand { get; private set; }

        #endregion

        public override void OnSuccessCommand(object parameter)
        {
            if (Album == null)
                return;

            IsBusy = true;

            Task<bool> saveAlbumTask = Task.Factory.StartNew<bool>(() =>
                {
                    return dataService.SaveAlbum(Album);
                }, TaskScheduler.Default);

            Task finishTask = saveAlbumTask.ContinueWith((t) =>
                {
                    IsBusy = false;

                    bool success = t.Result;

                    if (success)
                    {
                        Notify("Album has been successfully created/saved", NotificationType.Success);
                        RaiseReloadAlbums();
                    }
                    else
                    {
                        Notify("Can't save/create album. See log for details", NotificationType.Error);
                    }

                    DialogResult = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }


        #region Private methods

        private bool FilterTag(string search, object item)
        {
            Tag tag = item as Tag;
            if (tag != null)
            {
                string tagName = tag.Name;
                if (tagName != null && tagName.Contains(search))
                    return true;
            }

            return false;
        }

        private void OnAttachTagKeyboardCommand(KeyDownArgs parameter)
        {
            KeyEventArgs e = parameter.Settings;

            if (e != null && e.Key == Key.Return)
                OnAttachTagCommand(null);
        }

        private void OnAttachTagCommand(object parameter)
        {
            if (SelectedTag == null)
            {
                CreateNewTagAndAttach(NewTagName, Album);
            }
            else
            {
                AttachTag(SelectedTag);
            }

            SelectedTag = null;
            NewTagName = String.Empty;
        }

        private void CreateNewTagAndAttach(string tagName, Album album)
        {
            if (String.IsNullOrWhiteSpace(tagName) || album == null)
                return;

            Tag newTag = new Tag()
            {
                Name = tagName,
                CreateDate = DateTime.Now,
                Comments = "Created from Artist edit/create dialog"
            };

            ITagEditViewModel viewModel = PrepareTagEditOrCreate(newTag, false);

            if (viewModel.DialogResult == true)
            {
                AttachTag(viewModel.Tag);
            }
        }

        private ITagEditViewModel PrepareTagEditOrCreate(Tag editable, bool isEditMode)
        {
            ITagEditViewModel viewModel = container.Resolve<ITagEditViewModel>();
            viewModel.Tag = editable;
            viewModel.IsEditMode = isEditMode;
            viewModel.Tag.NeedValidate = true;

            TagEditView view = new TagEditView(viewModel);
            view.ShowDialog();

            return viewModel;
        }

        private void AttachTag(Tag tag)
        {
            if (tag == null)
                return;

            if (!AlreadyAttached(tag))
                Album.Tags.Add(tag);
            else
                Notify("Selected tag is already attached to this artist. All tags must be unique", NotificationType.Warning);
        }

        private bool AlreadyAttached(Tag tag)
        {
            foreach (Tag t in Album.Tags)
            {
                if (t == tag)
                    return true;
            }

            return false;
        }

        private void OnDetachTagCommand(object parameter)
        {
            Tag tag = parameter as Tag;
            if (tag != null)
            {
                Album.Tags.Remove(tag);
            }
        }

        private void OnEditTagCommand(MouseDoubleClickArgs parameter)
        {
            ListView tagsListView = parameter.Sender as ListView;

            if (tagsListView == null)
                return;

            Tag selectedTag = tagsListView.SelectedItem as Tag;

            if (selectedTag == null)
                return;

            PrepareTagEditOrCreate(selectedTag, true);
        }

        private void RaiseReloadAlbums()
        {
            eventAggregator.GetEvent<ReloadAlbumsEvent>().Publish(null);
        }

        private void OnTagsChangedEvent(object parameter)
        {
            LoadTags();
        }

        private void LoadTags()
        {
            Task<IList<Tag>> loadTagsTask = Task.Factory.StartNew<IList<Tag>>(() =>
            {
                return dataService.GetTags();
            }, TaskScheduler.Default);

            Task displayTask = loadTagsTask.ContinueWith((t) =>
            {
                Tags = t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnDropGenreCommand(DragArgs parameter)
        {
            if (parameter == null)
                return;

            if (parameter.Settings == null)
                return;

            Genre genre = parameter.Settings.Data.GetData(typeof(Genre)) as Genre;

            if (genre != null)
            {
                AttachGenreWithCheck(genre);
            }
        }

        private void AttachGenreWithCheck(Genre g)
        {
            if (Album == null || g == null)
                return;

            if (!Album.Genres.Contains(g))
            {
                Album.Genres.Add(g);
            }
        }

        private void OnSelectedGenresChangedCommand(MultiSelectionChangedArgs parameter)
        {
            if (parameter == null)
                return;

            SelectedGenres = parameter.GetSelectedValues<Genre>();
        }

        private void OnAttachGenresEvent(IList<Genre> genres)
        {
            if (genres == null || Album == null)
                return;

            foreach (Genre g in genres)
                AttachGenreWithCheck(g);
        }

        private void OnDetachGenresEvent(object parameter)
        {
            if (SelectedGenres == null || Album == null)
                return;

            foreach (Genre g in SelectedGenres)
            {
                Album.Genres.Remove(g);
            }
        }

        private void OnDetachGenreCommand(object parameter)
        {
            if (Album == null)
                return;

            Genre genre = parameter as Genre;

            if (genre == null)
                return;

            Album.Genres.Remove(genre);
        }

        private void OnDropArtistCommand(DragArgs parameter)
        {
            if (parameter == null)
                return;

            if (parameter.Settings == null)
                return;

            Artist artist = parameter.Settings.Data.GetData(typeof(Artist)) as Artist;

            if (artist != null)
            {
                AttachArtistWithCheck(artist);
            }
        }

        private void AttachArtistWithCheck(Artist artist)
        {
            if (Album == null || artist == null)
                return;

            Artist attachedArtist = Album.Artists.Where(a => a.ID == artist.ID).FirstOrDefault();

            if (attachedArtist == null)
            {
                Album.Artists.Add(artist);
            }
        }

        private void OnSelectedArtistsChangedCommand(MultiSelectionChangedArgs parameter)
        {
            if (parameter == null)
                return;

            SelectedArtists = parameter.GetSelectedValues<Artist>();
        }

        private void OnDetachArtistCommand(object parameter)
        {
            if (Album == null)
                return;

            Artist artist = parameter as Artist;

            if (artist == null)
                return;

            Artist attachedArtist = Album.Artists.Where(a => a.ID == artist.ID).FirstOrDefault();
            if (attachedArtist != null)
            {
                Album.Artists.Remove(attachedArtist);
            }
        }

        private void OnAttachArtistsEvent(IList<Artist> artists)
        {
            if (artists == null || Album == null)
                return;

            foreach (Artist a in artists)
                AttachArtistWithCheck(a);
        }

        private void OnDetachArtistsEvent(object parameter)
        {
            if (SelectedArtists == null || Album == null)
                return;

            foreach (Artist a in SelectedArtists)
            {
                Album.Artists.Remove(a);
            }
        }

        #endregion

        #region Private fields

        private IGenresListViewModel genresListViewModel;
        private IArtistListViewModel artistsListViewModel;
        private ITracksListViewModel trackListViewModel;

        private Album album;

        private IDataService dataService;

        private IList<Tag> tags;
        private Tag selectedTag;
        private string newTagName;

        private IList<Genre> selectedGenres;
        private IList<Artist> selectedArtists;

        #endregion
    }
}
