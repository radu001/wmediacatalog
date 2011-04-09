
using System.Collections;
using System.Collections.Generic;
using Prism.Wizards.Data;
namespace Prism.Wizards
{
    public class WizardSettings : IWizardSettings
    {
        public WizardSettings()
        {
            steps = new List<WizardStep>();
        }

        #region IWizardSettings Members

        public void AddStep<IViewModel, ViewModel, View>(int stepIndex, string stepName)
        {
            steps.Add(new WizardStep()
            {
                Index = stepIndex,
                Name = stepName,
                IViewModel = typeof(IViewModel),
                ViewModel = typeof(ViewModel),
                View = typeof(View)
            });
        }

        #endregion

        #region IEnumerable<WizardStep> Members

        public IEnumerator<WizardStep> GetEnumerator()
        {
            return steps.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return steps.GetEnumerator();
        }

        #endregion

        private List<WizardStep> steps;
    }
}
