
using System.Collections.Generic;
using Prism.Wizards.Data;
namespace Prism.Wizards
{
    internal interface IWizardContext : IEnumerable<WizardStep>
    {
        int StepsCount { get; }
        int CurrentStep { get; set; }
        WizardStep LastCompletedStep { get; set; }
    }
}
