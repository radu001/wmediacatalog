using System.Collections;
using System.Collections.Generic;
using Prism.Wizards.Data;

namespace Prism.Wizards
{
    internal class WizardContext : IWizardContext
    {
        public WizardContext(IWizardSettings settings)
        {
            stepsCollection = new WizardStepCollection();
            stepsCollection.AddRange(settings);
        }

        #region IWizardContext Members

        public int StepsCount
        {
            get
            {
                return stepsCollection.Count;
            }
        }

        public int CurrentStep { get; set; }

        public WizardStep LastCompletedStep { get; set; }

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
