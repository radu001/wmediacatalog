
using BusinessObjects;
using DataServices;
using System.Collections.Generic;
using TagCloudLib;
using Modules.Tags.Model;
namespace Modules.Tags.Services
{
    public class DataService : IDataService
    {
        public bool SaveTag(Tag tag)
        {
            var provider = new DataProvider();
            return provider.SaveTag(tag);
        }

        public IList<ITag> GetTagsWithAssociatedEntitiesCount()
        {
            var provider = new DataProvider();
            var tags = provider.GetTagsWithAssociatedEntitiesCount();

            var result = new List<ITag>();

            foreach (var t in tags)
            {
                var ta = new TagAdapter(t);
                result.Add(ta);
            }

            return result;
        }
    }
}
