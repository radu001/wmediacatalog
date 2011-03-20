using System;
using System.Collections.Generic;
using System.IO;

namespace MediaCatalog.Tests.Helpers
{
    public class DirectoryItem
    {
        public DirectoryInfo Dir { get; set; }

        public List<DirectoryItem> Childs { get; set; }

        public List<FileInfo> Files { get; set; }

        public DirectoryItem(DirectoryInfo dir)
        {
            Dir = dir;
            Childs = new List<DirectoryItem>();
            Files = new List<FileInfo>();
        }

        public DirectoryItem[] AddChilds(params string[] names)
        {
            if (names == null)
                throw new NullReferenceException();

            int dirCount = names.Length;

            DirectoryItem[] result = new DirectoryItem[dirCount];

            for (int i = 0; i < dirCount; ++i)
            {
                result[i] = new DirectoryItem(new DirectoryInfo(names[i]));
                Childs.Add(result[i]);
            }

            return result;
        }
    }
}
