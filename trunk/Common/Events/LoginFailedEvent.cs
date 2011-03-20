
using Common.Entities;
using Microsoft.Practices.Prism.Events;
namespace Common.Events
{
    public class LoginFailedEvent : CompositePresentationEvent<AuthorizationInfo>
    {
    }
}
