using System.IO;
using MediaCatalog.Tests.Helpers;
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
            container.RegisterType<IFileSystem, StubFileSystem>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITagsAccumulator, TagsAccumulator>(new ContainerControlledLifetimeManager());

            var fs = container.Resolve<IFileSystem>();
            var scanner = container.Resolve<IScanner>();
            var accumulator = container.Resolve<ITagsAccumulator>();

            //debug populate stub filesystem
            var root = ((StubFileSystem)fs).GetRoot();
            var childs = root.AddChilds("dir1", "dir2", "dir3");
            var childsDir3 = childs[2].AddChilds("dir31", "dir32");
            childsDir3[1].Files.Add(new FileInfo("test.flac"));



            string path = @"D:\";
            var service = new DataService(container);
            service.BeginScan(new ScanSettings()
            {
                ScanPath = path,
                FileMask = "*.flac"
            });
        }

        private Mockery mockery;
    }





}
