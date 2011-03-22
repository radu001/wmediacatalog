
using System.Collections.Generic;
using System.IO;
namespace MediaCatalog.Tests.Helpers
{
    public class DirectoryItem<T>
    {
        public DirectoryInfo Dir { get; set; }

        public DirectoryItem<T> Parent { get; set; }

        public List<DirectoryItem<T>> Childs { get; set; }

        public List<FileItem<T>> Files { get; set; }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        public DirectoryItem(DirectoryItem<T> parent)
        {
            Files = new List<FileItem<T>>();
            Childs = new List<DirectoryItem<T>>();
            Parent = parent;
        }
    }
}
