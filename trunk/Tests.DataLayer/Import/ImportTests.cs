using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services;
using Modules.Import.Services.Utils;
using NMock2;
using NUnit.Framework;

namespace MediaCatalog.Tests.Import
{
    [TestFixture]
    public class ImportTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            mockery = new Mockery();
        }

        [Test]
        public void TestScan()
        {
            var container = new UnityContainer();
            container.RegisterType<IScanner, VorbisCommentsScanner>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFileSystem, FileSystem>();
            container.RegisterType<ITagsAccumulator, TagsAccumulator>(new ContainerControlledLifetimeManager());

            var scanner = container.Resolve<IScanner>();
            var accumulator = container.Resolve<ITagsAccumulator>();

            string path = @"D:\Audio\";
            var service = new DataService(container);
            service.BeginScan(new ScanSettings()
            {
                ScanPath = path,
                FileMask = "*.flac"
            });
        }

        private Mockery mockery;
    }

    public class StubFileSystem : IFileSystem
    {
        public StubFileSystem(string rootPath)
        {
            root = new DirectoryItem(new DirectoryInfo(rootPath));
        }

        public DirectoryItem GetRoot()
        {
            return root;
        }

        #region IFileSystem Members

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private methods



        #endregion

        #region Private fields

        private DirectoryItem root;

        #endregion
    }

    public class DirectoryItem
    {
        public DirectoryInfo Dir { get; set; }

        public List<DirectoryInfo> Childs { get; set; }

        public List<FileInfo> Files { get; set; }

        public DirectoryItem(DirectoryInfo dir)
        {
            Dir = dir;
            Childs = new List<DirectoryInfo>();
            Files = new List<FileInfo>();
        }
    }

}
