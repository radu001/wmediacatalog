
using Microsoft.Practices.Prism.Events;
using Prism.Wizards.Data;
namespace Prism.Wizards.Events
{
    internal class WizardNavigationEvent : CompositePresentationEvent<NavigationSettings>
    {
    }

    internal class NavigationSettings
    {
        public string WizardName { get; set; }
        public bool MoveForward { get; set; }
        public WizardStep Step { get; set; }
    }
}
