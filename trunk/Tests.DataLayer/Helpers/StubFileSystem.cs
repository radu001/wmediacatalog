using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace MediaCatalog.Tests.Helpers
{
    public class StubFileSystem //: IFileSystem
    {
        public DirectoryItem Root { get; private set; }

        public StubFileSystem(string xml)
        {
            InitFileSystem(xml);
        }

        private void InitFileSystem(string xml)
        {
            XElement rootElement = XElement.Parse(xml);
            XAttribute rootAttribute = rootElement.Attribute("root");

            Root = new DirectoryItem(null)
            {
                Dir = new DirectoryInfo(rootAttribute.Value)
            };

            foreach (var child in rootElement.Elements("dir"))
                ProcessChild(child, Root);

            foreach (var childFile in rootElement.Elements("file"))
                Root.Files.Add(new FileInfo(childFile.Attribute("name").Value));
        }

        private void ProcessChild(XElement child, DirectoryItem parent)
        {
            var directory = new DirectoryItem(parent)
            {
                Dir = new DirectoryInfo(child.Attribute("name").Value)
            };
            parent.Childs.Add(directory);

            var files = child.Elements("file");
            foreach (var f in files)
                directory.Files.Add(new FileInfo(f.Attribute("name").Value));

            foreach (var d in child.Elements("dir"))
                ProcessChild(d, directory);
        }
    }

    public class DirectoryItem
    {
        public DirectoryInfo Dir { get; set; }

        public DirectoryItem Parent { get; set; }

        public List<DirectoryItem> Childs { get; set; }

        public List<FileInfo> Files { get; set; }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        public DirectoryItem(DirectoryItem parent)
        {
            Files = new List<FileInfo>();
            Childs = new List<DirectoryItem>();
            Parent = parent;
        }
    }
}
