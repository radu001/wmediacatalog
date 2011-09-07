
using BusinessObjects;
using System.Collections.Generic;
using TagCloudLib;
namespace Modules.Tags.Services
{
    public interface IDataService
    {
        bool SaveTag(Tag tag);
        IList<ITag> GetTagsWithAssociatedEntitiesCount();

    }
}
