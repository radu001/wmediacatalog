using Microsoft.Practices.Composite.Presentation.Events;

namespace Common.Events
{
    /// <summary>
    /// AlbumID is passed as payload
    /// </summary>
    public class EditAlbumEvent : CompositePresentationEvent<int>
    {
    }
}
