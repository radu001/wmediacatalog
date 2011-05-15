using System.IO;
using System.Linq;
using MediaCatalog.Tests.Mocks;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services;
using Modules.Import.Services.Utils;
using NUnit.Framework;

namespace MediaCatalog.Tests.TestSuites.Import
{
    [TestFixture]
    public class ImportTests
    {
        [Test]
        public void TestScan()
        {
            string xml = File.ReadAllText(@"FileSystems\FSTags.xml");

            var container = new UnityContainer();
            var resolver = new TagDataResolver();

            var scanner = new TransparentFileSystemMock(xml);
            var fileSelector = new StubFileSelector(scanner.GetDirectories());

            string path = @"d:\";
            var service = new DataService(container, scanner, scanner, new TagsAccumulator(), fileSelector);
            var artists =
                service.BeginScan(new ScanSettings()
                {
                    ScanPath = path,
                    FileMasks = new string[] { ".flac" }
                });

            Assert.NotNull(artists);
            Assert.AreEqual(1, artists.Count());
            Assert.AreEqual("Artist1", artists.SingleOrDefault().Name);

            var albums = artists.SingleOrDefault().Albums;
            Assert.NotNull(albums);
            Assert.AreEqual(2, albums.Count());

            var firstAlbum = albums.Where(a => a.Name == "Album1").FirstOrDefault();
            var secondAlbum = albums.Where(a => a.Name == "Album2").FirstOrDefault();

            Assert.NotNull(firstAlbum);
            Assert.NotNull(secondAlbum);
            Assert.AreSame(artists.SingleOrDefault(), firstAlbum.Artists[0]);
            Assert.AreSame(artists.SingleOrDefault(), secondAlbum.Artists[0]);
        }
    }





}
