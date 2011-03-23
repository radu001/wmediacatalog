
using System.Collections.Generic;
using BusinessObjects;
using Modules.Import.Model;
namespace Modules.Import.Services.Utils
{
    public interface ITagsAccumulator
    {
        void AccumulateTags(IEnumerable<FileTag> tags);
        IEnumerable<Artist> GetAccumulatedResult();
    }
}
