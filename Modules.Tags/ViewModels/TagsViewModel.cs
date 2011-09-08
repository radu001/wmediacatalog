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

namespace Modules.Tags.ViewModels
{
    public class TagsViewModel : ViewModelBase, ITagsViewModel
    {
        public TagsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;

            SubscribeEvents();

            SelectedTags = new ObservableCollection<ITag>();

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            SelectedTagsDropCommand = new DelegateCommand<object>(OnSelectedTagsDropCommand);
            AllTagsDropCommand = new DelegateCommand<object>(OnAllTagsDropCommand);
            SelectedTagsDragCommand = new DelegateCommand<object>(OnSelectedTagsDragCommand);
            AllTagsDragCommand = new DelegateCommand<object>(OnAllTagsDragCommand);
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

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> SelectedTagsDropCommand { get; private set; }

        public DelegateCommand<object> AllTagsDropCommand { get; private set; }

        public DelegateCommand<object> SelectedTagsDragCommand { get; private set; }

        public DelegateCommand<object> AllTagsDragCommand { get; private set; }

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

                LoadTags();
            }
        }

        private void LoadTags()
        {
            Tags = new ObservableCollection<ITag>(dataService.GetTagsWithAssociatedEntitiesCount());
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

                Filter();
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

                Filter();
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

        private void Filter()
        {
            var transformer = new IDListTransformer<ITag>();

            var idList = transformer.TransformIDs(SelectedTags, (o) =>
            {
                ITag tag = (ITag)o;
                return tag.ID;
            });

            //var options = new TagLoadOptions()
            //{
            //    FilterField = new Field("TagName","Tag name"),
            //    FilterValue = "",
            //    FirstResult = 0,
            //    MaxResults = 500
            //};

            var options = new TagLoadOptions()
            {
                FilterField = new Field("TagID", "Tag id"),
                FilterValue = idList,
                FirstResult = 0,
                MaxResults = 500
            };

            var list = dataService.GetTaggedObjects(options);
        }

        #endregion

        private void OnReloadTagsEvent(object parameter)
        {
            LoadTags();
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private IEventAggregator eventAggregator;

        private ObservableCollection<ITag> tags;
        private ObservableCollection<ITag> selectedTags;
        private ITag allTagsSelectedItem;
        private ITag selectedTagsSelectedItem;

        private bool draggingItem1;
        private bool draggingItem2;

        #endregion
    }
}
