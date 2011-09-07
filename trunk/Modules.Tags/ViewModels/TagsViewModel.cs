using Common.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Tags.Services;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using BusinessObjects;
using TagCloudLib;

namespace Modules.Tags.ViewModels
{
    public class TagsViewModel : ViewModelBase, ITagsViewModel
    {
        public TagsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            :base(container, eventAggregator)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            TagClickedCommand = new DelegateCommand<object>(OnTagClickedCommand);
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

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> TagClickedCommand { get; private set; }

        #endregion

        #region Private methods

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

        private void OnTagClickedCommand(object parameter)
        {
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private IEventAggregator eventAggregator;

        private ObservableCollection<ITag> tags;

        #endregion
    }
}
