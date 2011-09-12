using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BusinessObjects;
using BusinessObjects.Artificial;
using Common.Commands;
using Common.Controls.Controls;
using Common.Dialogs;
using Common.Dialogs.Helpers;
using Common.Entities.Pagination;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using DataServices;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Tags.Services;
using Modules.Tags.Views;
using TagCloudLib;

namespace Modules.Tags.ViewModels
{
    public class TagsViewModel : ViewModelBase, ITagsViewModel
    {
        public TagsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            SubscribeEvents();

            SelectedTags = new ObservableCollection<ITag>();

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            SelectedTagsDropCommand = new DelegateCommand<object>(OnSelectedTagsDropCommand);
            AllTagsDropCommand = new DelegateCommand<object>(OnAllTagsDropCommand);
            SelectedTagsDragCommand = new DelegateCommand<object>(OnSelectedTagsDragCommand);
            AllTagsDragCommand = new DelegateCommand<object>(OnAllTagsDragCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);
            TagDoubleClickedCommand = new DelegateCommand<object>(OnTagDoubleClickedCommand);
            MoveTagDownCommand = new DelegateCommand<object>(OnMoveTagDownCommand);
            MoveTagUpCommand = new DelegateCommand<object>(OnMoveTagUpCommand);
            ClearSelectedTagsCommand = new DelegateCommand<object>(OnClearSelectedTagsCommand);
        }

        #region ITagsViewModel Members

        public ObservableCollection<ITag> Tags
        {
            get
            {
                return tags;
            }
            private set
            {
                tags = value;
                NotifyPropertyChanged(() => Tags);
            }
        }

        public ObservableCollection<ITag> SelectedTags
        {
            get
            {
                return selectedTags;
            }
            private set
            {
                selectedTags = value;
                NotifyPropertyChanged(() => SelectedTags);
            }
        }

        public IPagedList<TaggedObject> TaggedObjects
        {
            get
            {
                return taggedObjects;
            }
            private set
            {
                taggedObjects = value;
                NotifyPropertyChanged(() => TaggedObjects);
            }
        }

        public ITag AllTagsSelectedItem
        {
            get
            {
                return allTagsSelectedItem;
            }
            set
            {
                allTagsSelectedItem = value;
            }
        }

        public ITag SelectedTagsSelectedItem
        {
            get
            {
                return selectedTagsSelectedItem;
            }
            set
            {
                selectedTagsSelectedItem = value;
            }
        }

        public ILoadOptions LoadOptions { get; private set; }

        public int TaggedObjectsCount
        {
            get
            {
                return taggedObjectsCount;
            }
            private set
            {
                if (taggedObjectsCount != value)
                {
                    taggedObjectsCount = value;
                    NotifyPropertyChanged(() => TaggedObjectsCount);
                }
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> SelectedTagsDropCommand { get; private set; }

        public DelegateCommand<object> AllTagsDropCommand { get; private set; }

        public DelegateCommand<object> SelectedTagsDragCommand { get; private set; }

        public DelegateCommand<object> AllTagsDragCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        public DelegateCommand<object> TagDoubleClickedCommand { get; private set; }

        public DelegateCommand<object> MoveTagUpCommand { get; private set; }

        public DelegateCommand<object> MoveTagDownCommand { get; private set; }

        public DelegateCommand<object> ClearSelectedTagsCommand { get; private set; }

        #endregion

        #region Private methods

        private void SubscribeEvents()
        {
            eventAggregator.GetEvent<ReloadTagsEvent>().Subscribe(OnReloadTagsEvent);
            eventAggregator.GetEvent<ReloadArtistsEvent>().Subscribe(OnReloadArtistsEvent);
            eventAggregator.GetEvent<ReloadAlbumsEvent>().Subscribe(OnReloadAlbumsEvent);
        }

        private void OnViewLoadedCommand(object parameter)
        {
            if (InitialDataLoaded)
                return;
            else
            {
                InitialDataLoaded = true;

                InitLoadOptions();
                LoadTags();
            }
        }

        private void LoadTags()
        {
            Tags = new ObservableCollection<ITag>(dataService.GetTagsWithAssociatedEntitiesCount());
            SelectedTags.Clear();
        }

        private void InitLoadOptions()
        {
            LoadOptions = new TagLoadOptions()
            {
                FilterField = new Field("TagID", "Tag id"),
                FilterValue = "",
                FirstResult = 0,
                MaxResults = 25,
                EntityName = ""
            };

            NotifyPropertyChanged(() => LoadOptions);
        }

        #region DragDrop

        private void OnAllTagsDropCommand(object parameter)
        {
            RemoveFromSelectedTags();
        }

        private void OnSelectedTagsDropCommand(object parameter)
        {
            MoveToSelectedTags();
        }

        private void OnSelectedTagsDragCommand(object parameter)
        {
            MouseMoveArgs args = parameter as MouseMoveArgs;
            if (args == null)
                return;

            var dragSource = args.Sender as TagsCloud;
            if (dragSource == null)
                return;

            if (args.Settings.LeftButton == MouseButtonState.Pressed)
            {
                if (SelectedTagsSelectedItem != null)
                {
                    DataObject dataObject = new DataObject(typeof(ITag), SelectedTagsSelectedItem);

                    DragDrop.DoDragDrop(dragSource, dataObject, DragDropEffects.Copy);

                    SelectedTagsSelectedItem = null;
                }
            }
        }

        private void OnAllTagsDragCommand(object parameter)
        {
            MouseMoveArgs args = parameter as MouseMoveArgs;
            if (args == null)
                return;

            var dragSource = args.Sender as TagsCloud;
            if (dragSource == null)
                return;

            if (args.Settings.LeftButton == MouseButtonState.Pressed)
            {
                if (AllTagsSelectedItem != null)
                {
                    DataObject dataObject = new DataObject(typeof(ITag), AllTagsSelectedItem);

                    DragDrop.DoDragDrop(dragSource, dataObject, DragDropEffects.Copy);

                    AllTagsSelectedItem = null;
                }
            }
        }

        #endregion

        #region Filtering

        private void FilterTaggedObjects()
        {
            if (SelectedTags == null || (SelectedTags != null && SelectedTags.Count == 0))
            {
                TaggedObjects = new PagedList<TaggedObject>();
                TaggedObjectsCount = 0;
            }
            else
            {
                var transformer = new IDListTransformer<ITag>();

                LoadOptions.FilterValue = transformer.TransformIDs(SelectedTags, (o) =>
                {
                    ITag tag = (ITag)o;
                    return tag.ID;
                });

                TaggedObjects = dataService.GetTaggedObjects(LoadOptions);
                TaggedObjectsCount = TaggedObjects.TotalItems;
            }
        }

        private void OnClearSelectedTagsCommand(object parameter)
        {
            if (SelectedTags == null)
                return;

            if (SelectedTags.Count == 0)
                return;

            foreach (var st in SelectedTags)
            {
                if (!Tags.Contains(st))
                    Tags.Add(st);
            }

            SelectedTags.Clear();

            FilterTaggedObjects();
        }

        private void MoveToSelectedTags()
        {
            if (AllTagsSelectedItem != null)
            {
                if (!SelectedTags.Contains(AllTagsSelectedItem))
                {
                    SelectedTags.Add(AllTagsSelectedItem);
                    Tags.Remove(AllTagsSelectedItem);

                    FilterTaggedObjects();
                }
            }
        }

        private void RemoveFromSelectedTags()
        {
            if (SelectedTagsSelectedItem != null)
            {
                if (!Tags.Contains(SelectedTagsSelectedItem))
                {
                    Tags.Add(SelectedTagsSelectedItem);
                    SelectedTags.Remove(SelectedTagsSelectedItem);

                    FilterTaggedObjects();
                }
            }
        }

        #endregion

        private void OnReloadTagsEvent(object parameter)
        {
            RefreshTags();
        }

        private void OnReloadArtistsEvent(object parameter)
        {
            //tags might be attached/detached
            RefreshTags();
        }

        private void OnReloadAlbumsEvent(object parameter)
        {
            //tags might be attached/detached
            RefreshTags();
        }

        private void RefreshTags()
        {
            LoadTags();
            FilterTaggedObjects();
        }

        private void OnPageChangedCommand(PageChangedArgs parameter)
        {
            if (parameter == null)
                return;

            PageChangedEventArgs e = parameter.Settings;

            if (LoadOptions != null && e != null)
            {
                LoadOptions.FirstResult = e.PageIndex * e.ItemsPerPage;
                FilterTaggedObjects();
            }
        }

        private void OnTagDoubleClickedCommand(object parameter)
        {
            ITag tagProxy = parameter as ITag;
            if (tagProxy == null)
                return;

            IsBusy = true;

            Task<Tag> loadTagTask = Task.Factory.StartNew<Tag>(() =>
            {
                return dataService.GetTag(tagProxy.ID);
            }, TaskScheduler.Default);

            Task finishTask = loadTagTask.ContinueWith((t) =>
            {
                IsBusy = false;

                Tag tag = t.Result;

                if (tag == null)
                    return;

                //TODO
                var viewModel = container.Resolve<ITagEditViewModel>();
                viewModel.Tag = tag;
                viewModel.IsEditMode = true;
                viewModel.Tag.NeedValidate = true;


                var dialog = new CommonDialog()
                {
                    DialogContent = new TagEditView(viewModel),
                    HeaderText = HeaderTextHelper.CreateHeaderText(typeof(Tag), true)
                };
                dialog.ShowDialog();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnMoveTagUpCommand(object parameter)
        {
            RemoveFromSelectedTags();
        }

        private void OnMoveTagDownCommand(object parameter)
        {
            MoveToSelectedTags();
        }

        #endregion

        #region Private fields

        private IDataService dataService;

        private ObservableCollection<ITag> tags;
        private ObservableCollection<ITag> selectedTags;
        private IPagedList<TaggedObject> taggedObjects;
        private ITag allTagsSelectedItem;
        private ITag selectedTagsSelectedItem;
        private int taggedObjectsCount;

        #endregion
    }
}
