
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
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Artists.Services;
using Modules.Tags.ViewModels;
using Modules.Tags.Views;
namespace Modules.Artists.ViewModels
{
    public class ArtistEditViewModel : DialogViewModelBase, IArtistEditViewModel, IEventSubscriber
    {
        public ArtistEditViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            SubscribeEvents();

            AttachTagCommand = new DelegateCommand<object>(OnAttachTagCommand);
            DetachTagCommand = new DelegateCommand<object>(OnDetachTagCommand);
            EditTagCommand = new DelegateCommand<MouseDoubleClickArgs>(OnEditTagCommand);
            CreateTagCommand = new DelegateCommand<object>(OnCreateTagCommand);

            LoadTags();
        }

        public override void OnSuccessCommand(object parameter)
        {
            if (Artist == null)
                return;

            if (!ValidateBeforeSave(parameter))
            {
                Notify("Please fill all required fields", NotificationType.Warning);
                return;
            }

            if (!IsEditMode && ArtistExists(Artist.Name))
            {
                Notify("Artist with given name already exists. Please specify another name", NotificationType.Error);
                return;
            }

            SaveArtistImpl();
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        public override void OnDialogClosingCommand(object parameter)
        {
            UnsubscribeEvents();
        }

        #region IEventSubscriber Members

        public void SubscribeEvents()
        {
            eventAggregator.GetEvent<TagsChangedEvent>().Subscribe(OnTagsChangedEvent, true);
        }

        public void UnsubscribeEvents()
        {
            eventAggregator.GetEvent<TagsChangedEvent>().Unsubscribe(OnTagsChangedEvent);
        }

        #endregion

        #region IArtistEditViewModel Members

        public Artist Artist
        {
            get
            {
                return artist;
            }
            set
            {
                artist = value;
                NotifyPropertyChanged(() => Artist);
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

        public Func<object, string, bool> FilterTag
        {
            get
            {
                return (t, s) =>
                {
                    var tag = t as Tag;
                    return tag.Name.ToUpper().Contains(s.ToUpper());
                };
            }
        }

        public DelegateCommand<object> AttachTagCommand { get; private set; }

        public DelegateCommand<object> DetachTagCommand { get; private set; }

        public DelegateCommand<object> CreateTagCommand { get; private set; }

        public DelegateCommand<MouseDoubleClickArgs> EditTagCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnCreateTagCommand(object parameter)
        {
            CreateNewTagAndAttach(TagName, Artist);
        }

        private void SaveArtistImpl()
        {
            IsBusy = true;

            Task<bool> saveArtistTask = Task.Factory.StartNew<bool>(() =>
            {
                return dataService.SaveArtist(Artist);
            }, TaskScheduler.Default);

            Task finishTask = saveArtistTask.ContinueWith((t) =>
            {
                IsBusy = false;

                bool success = t.Result;

                if (success)
                {
                    RaiseReloadArtists();
                    Notify("Artist has been successfully created (updated)", NotificationType.Success);
                }
                else
                {
                    Notify("Can't create or update artist. See log for details", NotificationType.Error);
                }

                DialogResult = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool ArtistExists(string artistName)
        {
            return dataService.ArtistExists(artistName);
        }

        private bool ValidateBeforeSave(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            return validator.Validate(parameter);
        }

        private void OnAttachTagCommand(object parameter)
        {
            var selectedTag = Tags.Where(t => t.Name == TagName).FirstOrDefault();

            if (selectedTag == null)
            {
                CreateNewTagAndAttach(TagName, Artist);
            }
            else
            {
                AttachTag(selectedTag);
            }
        }

        private void CreateNewTagAndAttach(string tagName, Artist artist)
        {
            if (String.IsNullOrWhiteSpace(tagName) || artist == null)
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

        private void OnDetachTagCommand(object parameter)
        {
            Tag tag = parameter as Tag;
            if (tag != null)
            {
                Artist.Tags.Remove(tag);
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
                Artist.Tags.Add(tag);
            else
                Notify("Selected tag is already attached to this artist. All tags must be unique", NotificationType.Warning);
        }

        private bool AlreadyAttached(Tag tag)
        {
            foreach (Tag t in Artist.Tags)
            {
                if (t == tag)
                    return true;
            }

            return false;
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

        private void RaiseReloadArtists()
        {
            eventAggregator.GetEvent<ReloadArtistsEvent>().Publish(null);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private Artist artist;
        private IList<Tag> tags;
        private string tagName;

        #endregion
    }
}
