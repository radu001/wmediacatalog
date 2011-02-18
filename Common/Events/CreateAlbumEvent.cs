
using Microsoft.Practices.Composite.Presentation.Events;
namespace Common.Events
{
    /// <summary>
    /// You may specify ArtistEntity as event payload. This way Artist will be attached to album
    /// </summary>
    public class CreateAlbumEvent : CompositePresentationEvent<object>
    {
    }
}
