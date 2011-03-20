
using System.Collections.Generic;
using BusinessObjects;
using Microsoft.Practices.Prism.Events;
namespace Modules.Albums.Events
{
    public class AttachGenresEvent : CompositePresentationEvent<IList<Genre>>
    {
    }
}
