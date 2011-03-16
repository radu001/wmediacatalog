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

            string path = @"D:\Audio\Blues\!Listening";
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
