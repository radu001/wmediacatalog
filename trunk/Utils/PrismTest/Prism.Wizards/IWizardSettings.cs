
using System.Collections.Generic;
using Prism.Wizards.Data;
namespace Prism.Wizards
{
    public interface IWizardSettings : IEnumerable<WizardStep>
    {
        void AddStep<IViewModel, ViewModel, View>(int stepIndex, string stepName);
        bool EnableNavBar { get; set; }
    }
}
