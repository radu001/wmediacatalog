using System.Collections.ObjectModel;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Tags.Services;
using TagCloudLib;
using Common.Commands;
using System.Windows.Input;
using System.Windows;
using Common.Entities.Pagination;
using Common.Utilities;
using BusinessObjects.Artificial;
using DataServices;
using Common.Controls.Controls;

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

        #endregion

        #region Private methods

        private void SubscribeEvents()
        {
            eventAggregator.GetEvent<ReloadTagsEvent>().Subscribe(OnReloadTagsEvent);
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
        }

        private void InitLoadOptions()
        {
            LoadOptions = new TagLoadOptions()
            {
                FilterField = new Field("TagID", "Tag id"),
                FilterValue = "",
                FirstResult = 0,
                MaxResults = 25,
            };

            NotifyPropertyChanged(() => LoadOptions);
        }

        #region DragDrop

        private void OnAllTagsDropCommand(object parameter)
        {
            if (SelectedTagsSelectedItem != null)
            {
                Tags.Add(SelectedTagsSelectedItem);
                SelectedTags.Remove(SelectedTagsSelectedItem);

                var control = GetTagsCloud(parameter); // update control due to itemsSource collection changed
                if (control != null) 
                    control.Refresh();

                FilterTaggedObjects();
            }   
        }

        private void OnSelectedTagsDropCommand(object parameter)
        {
            if (AllTagsSelectedItem != null)
            {
                SelectedTags.Add(AllTagsSelectedItem);
                Tags.Remove(AllTagsSelectedItem);

                var control = GetTagsCloud(parameter); // update control due to itemsSource collection changed
                if (control != null)
                    control.Refresh();

                FilterTaggedObjects();
            }
        }

        private TagsCloud GetTagsCloud(object parameter)
        {
            DragArgs da = parameter as DragArgs;
            if (da == null)
                return null;

            return da.Sender as TagsCloud;
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
                TaggedObjects = new PagedList<TaggedObject>();
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

        #endregion

        private void OnReloadTagsEvent(object parameter)
        {
            LoadTags();
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
