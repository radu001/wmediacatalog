using System.Collections.Generic;

namespace Modules.Import.Services.Utils.FileSystem
{
    public interface IFileSelector
    {
        void Init(IFileSelectorSettings settings);
        IEnumerable<FsFile> SelectFiles(string dirPath);
    }
}
