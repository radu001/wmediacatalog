
using Microsoft.Practices.Composite.Presentation.Events;
namespace Common.Events
{
    public class SwitchDbSettingsViewEvent : CompositePresentationEvent<DbSettingsMode>
    {
    }

    public enum DbSettingsMode
    {
        ConnectionSettings = 0,
        AdvancedSettings = 1
    }
}
