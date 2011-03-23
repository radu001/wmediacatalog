using System.IO;
using MediaCatalog.Tests.Mocks;
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
            string xml = File.ReadAllText(@"FileSystems\FSTags.xml");

            var container = new UnityContainer();
            var resolver = new TagDataResolver();

            var scanner = new StubTagsScanner(xml);

            string path = @"d:\";
            var service = new DataService(container, scanner, scanner, new TagsAccumulator());
            var artists =
                service.BeginScan(new ScanSettings()
                {
                    ScanPath = path,
                    FileMask = ".flac"
                });
        }

        private Mockery mockery;
    }





}
