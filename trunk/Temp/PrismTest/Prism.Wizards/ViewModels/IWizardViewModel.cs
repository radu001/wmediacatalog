
using System.Windows;
using Microsoft.Practices.Prism.Commands;
namespace Prism.Wizards.ViewModels
{
    public interface IWizardViewModel
    {
        string WizardName { get; set; }
        string WizardRegionName { get; set; }
        string StepRegionName { get; }
        Visibility NavBarVisible { get; set; }

        DelegateCommand<object> NextStepCommand { get; }
        DelegateCommand<object> PrevStepCommand { get; }
        DelegateCommand<object> MoveToStepCommand { get; }
    }
}
