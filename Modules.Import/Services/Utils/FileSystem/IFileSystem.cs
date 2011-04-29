using System.Collections.Generic;
using System.IO;

namespace Modules.Import.Services.Utils.FileSystem
{
    public interface IFileSystem
    {
        int CountFilesRecursively(DirectoryInfo dir, IFileSelector selector);
        IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, IFileSelector selector);
        IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir);
    }
}
