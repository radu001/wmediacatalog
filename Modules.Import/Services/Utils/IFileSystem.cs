using System.Collections.Generic;
using System.IO;

namespace Modules.Import.Services.Utils
{
    public interface IFileSystem
    {
        IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, string searchPattern);
        IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir);
    }
}
