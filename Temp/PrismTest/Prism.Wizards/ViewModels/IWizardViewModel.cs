
using Microsoft.Practices.Prism.Commands;
namespace Prism.Wizards.ViewModels
{
    public interface IWizardViewModel
    {
        string WizardName { get; set; }
        string WizardRegionName { get; set; }
        string StepRegionName { get; }

        DelegateCommand<object> NextStepCommand { get; }
        DelegateCommand<object> PrevStepCommand { get; }
    }
}
