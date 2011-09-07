
using BusinessObjects;
using DataServices;
using System.Collections.Generic;
namespace Modules.Tags.Services
{
    public class DataService : IDataService
    {
        public bool SaveTag(Tag tag)
        {
            var provider = new DataProvider();
            return provider.SaveTag(tag);
        }

        public IList<Tag> GetTagsWithAssociatedEntitiesCount()
        {
            var provider = new DataProvider();
            return provider.GetTagsWithAssociatedEntitiesCount();
        }
    }
}
