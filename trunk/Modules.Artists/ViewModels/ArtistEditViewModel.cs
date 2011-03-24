
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessObjects;
using Common.Commands;
using Common.Dialogs;
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
    public class ArtistEditViewModel : DialogViewModelBase, IArtistEditViewModel
    {
        public ArtistEditViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            eventAggregator.GetEvent<TagsChangedEvent>().Subscribe(OnTagsChangedEvent, true);

            FilterTagCommand = new AutoCompleteFilterPredicate<object>(FilterTag);
            AttachTagCommand = new DelegateCommand<object>(OnAttachTagCommand);
            AttachTagKeyboardCommand = new DelegateCommand<KeyDownArgs>(OnAttachTagKeyboardCommand);
            DetachTagCommand = new DelegateCommand<object>(OnDetachTagCommand);
            EditTagCommand = new DelegateCommand<MouseDoubleClickArgs>(OnEditTagCommand);

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

        public AutoCompleteFilterPredicate<object> FilterTagCommand { get; private set; }

        public DelegateCommand<object> AttachTagCommand { get; private set; }

        public DelegateCommand<KeyDownArgs> AttachTagKeyboardCommand { get; private set; }

        public DelegateCommand<object> DetachTagCommand { get; private set; }

        public DelegateCommand<MouseDoubleClickArgs> EditTagCommand { get; private set; }

        #endregion

        #region Private methods

        private void SaveArtistImpl()
        {
            IsBusy = true;

            Task<bool> saveArtistTask = Task.Factory.StartNew<bool>(() =>
            {
                Thread.Sleep(10000);
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
                CreateNewTagAndAttach(NewTagName, Artist);
            }
            else
            {
                AttachTag(SelectedTag);
            }

            SelectedTag = null;
            NewTagName = String.Empty;
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

            ITagEditViewModel viewModel = PrepareTagEditOrCreate(newTag, false);

            if (viewModel.DialogResult == true)
            {
                AttachTag(viewModel.Tag);
            }
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

            PrepareTagEditOrCreate(selectedTag, true);
        }

        private ITagEditViewModel PrepareTagEditOrCreate(Tag editable, bool isEditMode)
        {
            ITagEditViewModel viewModel = container.Resolve<ITagEditViewModel>();
            viewModel.Tag = editable;
            viewModel.IsEditMode = isEditMode;
            viewModel.Tag.NeedValidate = true;


            CommonDialog dialog = new CommonDialog()
            {
                DialogContent = new TagEditView(viewModel)
            };
            dialog.ShowDialog();

            return viewModel;
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
        private Tag selectedTag;
        private string newTagName;

        #endregion
    }
}
