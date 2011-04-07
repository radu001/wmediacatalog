
using System.Collections.Generic;
using Prism.Wizards.Data;
namespace Prism.Wizards
{
    public interface IWizardContext : IEnumerable<WizardStep>
    {
        int StepsCount { get; }
        int CurrentStep { get; set; }

        void AddStep<IViewModel, ViewModel, View>(int stepIndex, string stepName);
    }
}
