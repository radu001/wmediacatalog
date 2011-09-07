using Common.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Tags.Services;
using Microsoft.Practices.Prism.Commands;

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
        }

        #region ITagsViewModel Members

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnViewLoadedCommand(object parameter)
        {
            if (InitialDataLoaded)
                return;
            else
            {
                InitialDataLoaded = true;
                //Do load data here for first time we open view
            }
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private IEventAggregator eventAggregator;

        #endregion
    }
}
