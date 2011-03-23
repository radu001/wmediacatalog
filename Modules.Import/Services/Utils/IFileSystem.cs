using System.Collections.Generic;
using System.IO;

namespace Modules.Import.Services.Utils
{
    public interface IFileSystem
    {
        int CountFilesRecursively(DirectoryInfo dir, string searchPattern);
        IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, string searchPattern);
        IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir);
    }
}
