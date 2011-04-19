
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Prism.Wizards;
namespace PrismTest.Module.ViewModels
{
    public class Step2ViewModel : ViewModelBase, IStep2ViewModel
    {
        public Step2ViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            var data = container.Resolve<IWizardData>();
        }
    }
}
