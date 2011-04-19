
using Microsoft.Practices.Prism.Events;
namespace PrismTest.Module.ViewModels
{
    public class InitialViewModel : ViewModelBase, IInitialViewModel
    {
        public InitialViewModel(IEventAggregator eventAggregator)
            :base(eventAggregator)
        {
        }
    }
}
