using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;

namespace Modules.DatabaseSettings.ViewModels
{
    public class DatabaseToolsViewModel : ViewModelBase, IDatabaseToolsViewModel
    {
        public DatabaseToolsViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
        }
    }
}
