using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Modules.Import.Services.Utils
{
    public class FileSystem : IFileSystem
    {
        #region IFileSystem Members

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, string searchPattern)
        {
            return dir.GetFiles(searchPattern);
        }

        public IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir)
        {
            return dir.GetDirectories();
        }

        public int CountFilesRecursively(DirectoryInfo dir, string searchPattern)
        {
            return dir.EnumerateFiles(searchPattern, SearchOption.AllDirectories).Count();
        }

        #endregion
    }
}
