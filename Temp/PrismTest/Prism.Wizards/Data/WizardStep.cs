
using System;
namespace Prism.Wizards.Data
{
    public class WizardStep
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCurrent { get; set; }

        public Type IViewModel { get; set; }
        public Type ViewModel { get; set; }
        public Type View { get; set; }
    }
}
