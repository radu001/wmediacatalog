
using Microsoft.Practices.Prism.Events;
using Modules.Main.Services;
namespace Modules.Main.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        public MainViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            this.eventAggregator = eventAggregator;
            this.dataService = dataService;
        }

        private IEventAggregator eventAggregator;
        private IDataService dataService;
    }
}
