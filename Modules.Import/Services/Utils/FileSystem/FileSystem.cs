using System;
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
            return selector.SelectFiles(dir.FullName);
        }

        public IEnumerable<Dir> GetSubDirectories(Dir dir)
        {
            return dir.GetDirectories();
        }

        public int CountFilesRecursively(Dir dir, IFileSelector selector)
        {
            if (dir == null)
                throw new NullReferenceException("Illegal null-reference dir");

            if (!Directory.Exists(dir.FullName))
                return 0;

            int result = 0;

            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(dir.FullName));

            while (stack.Count > 0)
            {
                DirectoryInfo top = stack.Pop();

                var subDirs = top.GetDirectories();
                foreach (var sd in subDirs)
                    stack.Push(sd);

                result += selector.SelectFiles(top.FullName).Count();
            }

            return result;
        }

        #endregion
    }
}
