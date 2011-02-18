using Common.Entities;
using Microsoft.Practices.Composite.Presentation.Events;

namespace Common.Events
{
    public class LoginSucceededEvent : CompositePresentationEvent<AuthorizationInfo>
    {
    }
}
