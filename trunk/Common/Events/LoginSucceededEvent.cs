using Common.Entities;
using Microsoft.Practices.Prism.Events;

namespace Common.Events
{
    public class LoginSucceededEvent : CompositePresentationEvent<AuthorizationInfo>
    {
    }
}
