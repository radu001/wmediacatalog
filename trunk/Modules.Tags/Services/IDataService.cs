
using BusinessObjects;
using System.Collections.Generic;
namespace Modules.Tags.Services
{
    public interface IDataService
    {
        bool SaveTag(Tag tag);
        IList<Tag> GetTagsWithAssociatedEntitiesCount();
    }
}
