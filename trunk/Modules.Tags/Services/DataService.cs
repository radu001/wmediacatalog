
using BusinessObjects;
using DataServices;
namespace Modules.Tags.Services
{
    public class DataService : IDataService
    {
        public bool SaveTag(Tag tag)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveTag(tag);
        }
    }
}
