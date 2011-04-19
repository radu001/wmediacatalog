
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Prism.Wizards;
namespace PrismTest.Module.ViewModels
{
    public class Step1ViewModel : ViewModelBase, IStep1ViewModel
    {
        public Step1ViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            :base(eventAggregator)
        {
            var data = container.Resolve<IWizardData>();
            data.SetValue<string>("123123");
        }

    }
}
