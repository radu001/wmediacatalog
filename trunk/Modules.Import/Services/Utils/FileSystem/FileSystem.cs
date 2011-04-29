using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Modules.Import.Services.Utils.FileSystem
{
    public class FileSystem : IFileSystem
    {
        #region IFileSystem Members

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, IFileSelector selector)
        {
            return selector.SelectFiles(dir);
        }

        public IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir)
        {
            return dir.GetDirectories();
        }

        public int CountFilesRecursively(DirectoryInfo dir, IFileSelector selector)
        {
            int result = 0;

            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();
            stack.Push(dir);

            while (stack.Count > 0)
            {
                DirectoryInfo top = stack.Pop();

                var subDirs = top.GetDirectories();
                foreach (var sd in subDirs)
                    stack.Push(sd);

                result += selector.SelectFiles(top).Count();
            }

            return result;
        }

        #endregion
    }
}
