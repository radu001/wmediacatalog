using System.Collections.Generic;
using System.IO;
using System.Linq;
using Modules.Import.Services.Utils;

namespace MediaCatalog.Tests.Helpers
{
    public class StubFileSystem : IFileSystem
    {
        public StubFileSystem(string rootPath)
        {
            root = new DirectoryItem(new DirectoryInfo((rootPath)));
        }

        public DirectoryItem GetRoot()
        {
            return root;
        }

        #region IFileSystem Members

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, string searchPattern)
        {
            var parent = FindDicrectory(dir);
            if (parent != null)
            {
                return parent.Files.Where(f => f.Name.Contains(searchPattern));
            }

            return new FileInfo[] { };
        }

        public IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir)
        {
            var parent = FindDicrectory(dir);
            if (parent != null)
                return parent.Childs.Select(ch => ch.Dir);

            return new DirectoryInfo[] { };
        }

        #endregion

        #region Private methods

        private DirectoryItem FindDicrectory(DirectoryInfo dir)
        {
            if (dir == null)
                return null;

            DirectoryItem result = null;

            Stack<DirectoryItem> stack = new Stack<DirectoryItem>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (current.Dir.FullName == dir.FullName)
                {
                    result = current;
                    break;
                }
                else
                {
                    foreach (var child in current.Childs)
                        stack.Push(child);
                }
            }

            return result;
        }

        #endregion

        #region Private fields

        private DirectoryItem root;

        #endregion
    }
}
