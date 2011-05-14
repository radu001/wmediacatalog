using System.Collections.Generic;
using System.IO;

namespace Modules.Import.Services.Utils.FileSystem
{
    public interface IFileSystem
    {
        int CountFilesRecursively(Dir dir, IFileSelector selector);
        IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, IFileSelector selector);
        IEnumerable<Dir> GetSubDirectories(Dir dir);
    }
}
