
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using BusinessObjects;
using Common.Commands;
using Common.Dialogs;
using Common.Dialogs.Helpers;
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
    public class AlbumEditViewModel : DialogViewModelBase, IAlbumEditViewModel, IEventSubscriber
    {
        public AlbumEditViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            SubscribeEvents();

            AttachTagCommand = new DelegateCommand<object>(OnAttachTagCommand);
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

        #region IEventSubscriber Members

        public void SubscribeEvents()
        {
            eventAggregator.GetEvent<TagsChangedEvent>().Subscribe(OnTagsChangedEvent, true);
            eventAggregator.GetEvent<AttachGenresEvent>().Subscribe(OnAttachGenresEvent, true);
            eventAggregator.GetEvent<DetachGenresEvent>().Subscribe(OnDetachGenresEvent, true);
            eventAggregator.GetEvent<AttachArtistsEvent>().Subscribe(OnAttachArtistsEvent, true);
            eventAggregator.GetEvent<DetachArtistsEvent>().Subscribe(OnDetachArtistsEvent, true);
        }

        public void UnsubscribeEvents()
        {
            eventAggregator.GetEvent<TagsChangedEvent>().Unsubscribe(OnTagsChangedEvent);
            eventAggregator.GetEvent<AttachGenresEvent>().Unsubscribe(OnAttachGenresEvent);
            eventAggregator.GetEvent<DetachGenresEvent>().Unsubscribe(OnDetachGenresEvent);
            eventAggregator.GetEvent<AttachArtistsEvent>().Unsubscribe(OnAttachArtistsEvent);
            eventAggregator.GetEvent<DetachArtistsEvent>().Unsubscribe(OnDetachArtistsEvent);

            GenresListViewModel.UnsubscribeEvents();
            ArtistsListViewModel.UnsubscribeEvents();
        }

        #endregion

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

        public Func<object, string, bool> FilterTag
        {
            get
            {
                return (t, s) =>
                {
                    var tag = t as Tag;
                    return tag.Name.Contains(s);
                };
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

        public string TagName
        {
            get
            {
                return tagName;
            }
            set
            {
                tagName = value;
                NotifyPropertyChanged(() => TagName);
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

        public DelegateCommand<object> AttachTagCommand { get; private set; }

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

            if (!ValidateBusinessRules(Album))
            {
                Notify("Album should have non-empty name and at least one artist and one genre attached. " +
                       "Please attach at least one artist or genre to album", NotificationType.Warning);
                return;
            }

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

        public override void OnDialogClosingCommand(object parameter)
        {
            UnsubscribeEvents();
        }

        #region Private methods

        private void OnAttachTagCommand(object parameter)
        {
            var selectedTag = Tags.Where(t => t.Name == TagName).FirstOrDefault();

            if (selectedTag == null)
            {
                CreateNewTagAndAttach(TagName, Album);
            }
            else
            {
                AttachTag(selectedTag);
            }
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

            IsBusy = true;

            Task<bool> saveTagTask = Task.Factory.StartNew<bool>(() =>
            {
                return dataService.SaveTag(newTag);
            }, TaskScheduler.Default);

            Task attachTask = saveTagTask.ContinueWith((t) =>
            {
                IsBusy = false;

                if (t.Result)
                {
                    AttachTag(newTag);
                    LoadTags();
                }
                else
                {
                    Notify("Error while saving new tag into database. See log for details",
                        NotificationType.Error);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void EditTag(Tag editable)
        {
            var viewModel = container.Resolve<ITagEditViewModel>();
            viewModel.Tag = editable;
            viewModel.IsEditMode = true;
            viewModel.Tag.NeedValidate = true;

            var dialog = new CommonDialog()
            {
                DialogContent = new TagEditView(viewModel),
                HeaderText = HeaderTextHelper.CreateHeaderText(typeof(Tag), true)
            };
            dialog.ShowDialog();
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
            return Album.Tags.Any(t => t.Name == tag.Name);
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

            EditTag(selectedTag);
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

        private bool ValidateBusinessRules(Album album)
        {
            if (album != null)
            {
                if (String.IsNullOrEmpty(album.Name))
                    return false;

                if (album.Genres.Count > 0 && album.Artists.Count > 0)
                    return true;
            }
            return false;
        }

        #endregion

        #region Private fields

        private IGenresListViewModel genresListViewModel;
        private IArtistListViewModel artistsListViewModel;
        private ITracksListViewModel trackListViewModel;

        private Album album;

        private IDataService dataService;

        private IList<Tag> tags;
        private string tagName;

        private IList<Genre> selectedGenres;
        private IList<Artist> selectedArtists;

        #endregion
    }
}
