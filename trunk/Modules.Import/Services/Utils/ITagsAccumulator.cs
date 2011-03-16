
using System.Collections.Generic;
using Modules.Import.Model;
namespace Modules.Import.Services.Utils
{
    public interface ITagsAccumulator
    {
        void AccumulateTags(IEnumerable<FileTag> tags);
    }
}
