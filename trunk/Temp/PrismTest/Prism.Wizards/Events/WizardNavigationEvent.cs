
using Microsoft.Practices.Prism.Events;
namespace Prism.Wizards.Events
{
    public class WizardNavigationEvent : CompositePresentationEvent<NavigationSettings>
    {
    }

    public class NavigationSettings
    {
        public string WizardName { get; set; }
        public bool MoveForward { get; set; }
    }
}
