
using Microsoft.Practices.Prism.Events;
namespace PrismTest.Module.ViewModels
{
    public class Step1ViewModel : ViewModelBase, IStep1ViewModel
    {
        public Step1ViewModel(IEventAggregator eventAggregator)
            :base(eventAggregator)
        {
        }
    }
}
