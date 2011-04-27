using System.Collections.Generic;

namespace TagCloudLib
{
    public interface ITagProvider
    {
        IEnumerable<ITag> GetTags();
    }
}
