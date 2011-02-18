
using Microsoft.Practices.Composite.Presentation.Events;
namespace Common.Events
{
    /// <summary>
    /// Occurs when album is being loaded from database just before edit/view
    /// </summary>
    public class AlbumLoadedEvent : CompositePresentationEvent<int>
    {
    }
}
