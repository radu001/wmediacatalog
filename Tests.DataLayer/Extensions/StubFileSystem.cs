using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Modules.Import.Services.Utils;

namespace MediaCatalog.Tests.Extensions
{
    public class StubFileSystem<T> : IFileSystem
    {
        public DirectoryItem<T> Root { get; private set; }

        public IFileDataResolver<T> FileDataResolver { get; private set; }

        public StubFileSystem(string xml)
            : this(xml, new DefaultFileDataResolver<T>())
        {
        }

        public StubFileSystem(string xml, IFileDataResolver<T> fileDataResolver)
        {
            if (fileDataResolver == null)
                throw new NullReferenceException("Illegal null-reference file data resolver");

            FileDataResolver = fileDataResolver;

            InitFileSystem(xml);
        }

        public DirectoryItem<T> FindDirectory(string path)
        {
            if (Root.Dir.FullName == path)
                return Root;

            Stack<DirectoryItem<T>> stack = new Stack<DirectoryItem<T>>();
            AppendToStack(stack, Root.Childs);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (current.Dir.FullName == path)
                    return current;
                else
                {
                    AppendToStack(stack, current.Childs);
                }
            }

            return null;
        }

        public T GetFileData(string fullFileName)
        {
            string path = Path.GetDirectoryName(fullFileName);
            string fileName = Path.GetFileName(fullFileName);

            var dir = FindDirectory(path);
            if (dir != null)
            {
                var file = dir.Files.Where(f => f.File.FullName == fullFileName).FirstOrDefault();
                if (file != null)
                    return file.Data;
            }

            return default(T);
        }

        #region IFileSystem Members

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, string searchPattern)
        {
            var directory = FindDirectory(dir.FullName);
            if (directory == null)
                return new FileInfo[] { };

            return directory.Files.Select(f => f.File).Where(f => f.Name.Contains(searchPattern));
        }

        public IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir)
        {
            var directory = FindDirectory(dir.FullName);
            if (directory == null)
                return new DirectoryInfo[] { };

            return directory.Childs.Select(c => c.Dir);
        }

        #endregion

        #region Protected methods

        protected T GetFileData(XElement fileElement)
        {
            return FileDataResolver.GetFileData(fileElement);
        }

        #endregion

        #region Private methods

        private void InitFileSystem(string xml)
        {
            XElement rootElement = XElement.Parse(xml);
            XAttribute rootAttribute = rootElement.Attribute("root");

            Root = new DirectoryItem<T>(null)
            {
                Dir = new DirectoryInfo(rootAttribute.Value)
            };

            foreach (var child in rootElement.Elements("dir"))
                ProcessChild(child, Root);

            foreach (var childFile in rootElement.Elements("file"))
                ProcessChildFile(Root, Root, childFile);
        }

        private void ProcessChild(XElement child, DirectoryItem<T> parent)
        {
            string directoryName = CreateDirectoryName(parent, child);
            var directory = AddSubdirectory(parent, directoryName);

            var files = child.Elements("file");
            foreach (var f in files)
            {
                ProcessChildFile(parent, directory, f);
            }

            foreach (var d in child.Elements("dir"))
                ProcessChild(d, directory);
        }

        private void ProcessChildFile(DirectoryItem<T> parent, DirectoryItem<T> directory, XElement f)
        {
            var fileName = CreateFileName(parent, f);
            var fileData = GetFileData(f);
            AddFileToDirectory(directory, fileName, fileData);
        }

        private DirectoryItem<T> AddSubdirectory(DirectoryItem<T> parent, string directoryName)
        {
            int sameDirsCount = parent.Childs.Select(c => c.Dir.FullName).
                Where(s => s == directoryName).Count();

            if (sameDirsCount == 0)
            {
                var subDir = new DirectoryItem<T>(parent)
                {
                    Dir = new DirectoryInfo(directoryName)
                };

                parent.Childs.Add(subDir);

                return subDir;
            }
            else
            {
                throw new Exception(); // duplicate detected
            }
        }

        private void AddFileToDirectory(DirectoryItem<T> dir, string fullFileName, T fileData)
        {
            int sameFilesCount = dir.Files.Select(f => f.File).Where(f => f.FullName == fullFileName).Count();
            if (sameFilesCount == 0)
            {
                dir.Files.Add(new FileItem<T>()
                {
                    File = new FileInfo(fullFileName),
                    Data = fileData
                });
            }
            else
            {
                throw new Exception(
                    String.Format("Attempted to add duplicate file={0} in the same directory={1}.",
                    dir.Dir.FullName, fullFileName));
            }
        }

        private string CreateDirectoryName(DirectoryItem<T> parent, XElement dirElement)
        {
            return Path.Combine(parent.Dir.FullName, dirElement.Attribute("name").Value);
        }

        private string CreateFileName(DirectoryItem<T> parent, XElement fileElement)
        {
            return Path.Combine(parent.Dir.FullName, fileElement.Attribute("name").Value);
        }

        private void AppendToStack(Stack<DirectoryItem<T>> stack, IEnumerable<DirectoryItem<T>> items)
        {
            foreach (var i in items)
            {
                stack.Push(i);
            }
        }

        #endregion
    }
}
