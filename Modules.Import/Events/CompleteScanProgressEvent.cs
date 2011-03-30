
using System.Collections.Generic;
using BusinessObjects;
using Microsoft.Practices.Prism.Events;
namespace Modules.Import.Events
{
    public class CompleteScanProgressEvent : CompositePresentationEvent<IEnumerable<Artist>>
    {
    }
}
