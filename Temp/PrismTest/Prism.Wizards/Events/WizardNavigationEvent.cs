
using Microsoft.Practices.Prism.Events;
using Prism.Wizards.Data;
namespace Prism.Wizards.Events
{
    public class WizardNavigationEvent : CompositePresentationEvent<NavigationSettings>
    {
    }

    public class NavigationSettings
    {
        public string WizardName { get; set; }
        public bool MoveForward { get; set; }
        public WizardStep Step { get; set; }
    }
}
