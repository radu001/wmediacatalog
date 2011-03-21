using System.IO;
using MediaCatalog.Tests.Helpers;
using NUnit.Framework;

namespace MediaCatalog.Tests.Import
{
    [TestFixture]
    public class StubFileSystemTest
    {
        [Test]
        public void CreateFs_ValidXml_Success()
        {
            var xml = File.ReadAllText(@"FileSystems\FS1.xml");
            StubFileSystem fs = new StubFileSystem(xml);

            Assert.True(fs.Root.IsRoot);
            Assert.AreEqual(@"d:\", fs.Root.Dir.Name);


        }
    }
}
