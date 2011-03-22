using System.IO;
using MediaCatalog.Tests.Helpers;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services;
using Modules.Import.Services.Utils;
using NMock2;
using NUnit.Framework;

namespace MediaCatalog.Tests.TestSuites.Import
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
            string xml = File.ReadAllText(@"FileSystems\FS1.xml");

            var container = new UnityContainer();

            string path = @"d:\";
            var service = new DataService(container, new StubTagsScanner(), new StubFileSystem<object>(xml), new TagsAccumulator());
            var artists = 
                service.BeginScan(new ScanSettings()
                {
                    ScanPath = path,
                    FileMask = ".png"
                });
        }

        private Mockery mockery;
    }





}
