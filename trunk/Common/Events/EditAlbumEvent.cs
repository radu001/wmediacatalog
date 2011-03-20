using Microsoft.Practices.Prism.Events;

namespace Common.Events
{
    /// <summary>
    /// AlbumID is passed as payload
    /// </summary>
    public class EditAlbumEvent : CompositePresentationEvent<int>
    {
    }
}
