using System.Collections;
using System.Collections.Generic;
using Prism.Wizards.Data;

namespace Prism.Wizards
{
    public class WizardContext : IWizardContext
    {
        public WizardContext()
        {
            stepsCollection = new WizardStepCollection();
        }

        #region IWizardContext Members

        public int StepsCount
        {
            get
            {
                return stepsCollection.Count;
            }
        }

        public void AddStep<IViewModel, ViewModel, View>(int stepIndex, string stepName)
        {
            WizardStep step = new WizardStep()
            {
                Index = stepIndex,
                Name = stepName,
                IViewModel = typeof(IViewModel),
                ViewModel = typeof(ViewModel),
                View = typeof(View)
            };

            stepsCollection.Add(step);
        }

        #endregion

        #region IEnumerable<WizardStep> Members

        public IEnumerator<WizardStep> GetEnumerator()
        {
            return stepsCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return stepsCollection.GetEnumerator();
        }

        #endregion

        private WizardStepCollection stepsCollection;
    }
}
