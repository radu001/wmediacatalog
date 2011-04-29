using System.Collections.Generic;
using System.IO;

namespace Modules.Import.Services.Utils.FileSystem
{
    public interface IFileSelector
    {
        void Init(IFileSelectorSettings settings);
        IEnumerable<FileInfo> SelectFiles(DirectoryInfo dir);
    }
}
