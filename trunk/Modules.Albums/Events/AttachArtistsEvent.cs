using System.Collections.Generic;
using BusinessObjects;
using Microsoft.Practices.Composite.Presentation.Events;

namespace Modules.Albums.Events
{
    public class AttachArtistsEvent : CompositePresentationEvent<IList<Artist>>
    {
    }
}
