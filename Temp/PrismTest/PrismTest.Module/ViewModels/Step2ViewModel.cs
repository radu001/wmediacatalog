
using Microsoft.Practices.Prism.Events;
namespace PrismTest.Module.ViewModels
{
    public class Step2ViewModel : ViewModelBase, IStep2ViewModel
    {
        public Step2ViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
