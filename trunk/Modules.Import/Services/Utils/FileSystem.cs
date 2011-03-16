using System.Collections.Generic;
using System.IO;

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

        #endregion
    }
}
