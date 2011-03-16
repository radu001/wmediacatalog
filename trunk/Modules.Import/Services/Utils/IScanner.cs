
using System.Collections.Generic;
using Modules.Import.Model;
namespace Modules.Import.Services.Utils
{
    public interface IScanner
    {
        IEnumerable<FileTag> GetTags(string filePath);
    }
}
