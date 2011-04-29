using System;
using System.IO;
using System.Linq;
using MediaCatalog.Tests.Extensions;
using MediaCatalog.Tests.Mocks;
using Modules.Import.Services.Utils.FileSystem;
using NUnit.Framework;

namespace MediaCatalog.Tests.TestSuites.Import
{
    [TestFixture]
    public class StubFileSystemTest
    {
        [Test]
        public void CreateFs_ValidXml_Success()
        {
            var xml = File.ReadAllText(@"FileSystems\FS1.xml");
            var fs = CreateFileSystem(xml) as StubFileSystem<object>;

            Assert.True(fs.Root.IsRoot);
            Assert.AreEqual(@"d:\", fs.Root.Dir.Name);

            Assert.AreEqual(1, fs.Root.Childs.Count);
            AssertHasFiles(fs.Root, "file3.png", "file4.png", "file5.png");
            AssertHasFolders(fs.Root, @"d:\dir1");

            var dir1 = fs.FindDirectory(@"d:\dir1");
            Assert.NotNull(dir1);
            Assert.AreSame(fs.Root, dir1.Parent);
            AssertHasFolders(dir1, @"d:\dir1\dir2", @"d:\dir1\dir3", @"d:\dir1\dir4");
            AssertHasNoFiles(dir1);

            var dir2 = fs.FindDirectory(@"d:\dir1\dir2");
            Assert.NotNull(dir2);
            Assert.AreSame(dir1, dir2.Parent);
            AssertHasNoFolders(dir2);
            AssertHasNoFiles(dir2);

            var dir3 = fs.FindDirectory(@"d:\dir1\dir3");
            Assert.NotNull(dir3);
            Assert.AreSame(dir1, dir3.Parent);
            AssertHasNoFolders(dir3);
            AssertHasNoFiles(dir3);

            var dir4 = fs.FindDirectory(@"d:\dir1\dir4");
            Assert.NotNull(dir4);
            Assert.AreSame(dir1, dir4.Parent);
            AssertHasFiles(dir4, "file.png", "file1.png");
            AssertHasFolders(dir4, @"d:\dir1\dir4\dir5");

            var dir5 = fs.FindDirectory(@"d:\dir1\dir4\dir5");
            Assert.NotNull(dir5);
            Assert.AreSame(dir4, dir5.Parent);
            AssertHasFiles(dir5, "file6.png");
            AssertHasFolders(dir5, @"d:\dir1\dir4\dir5\dir6");

            var dir6 = fs.FindDirectory(@"d:\dir1\dir4\dir5\dir6");
            Assert.NotNull(dir6);
            Assert.AreSame(dir5, dir6.Parent);
            AssertHasNoFolders(dir6);
            AssertHasNoFiles(dir6);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CreateFs_DuplicateFilesInFolder_ThrowsException()
        {
            var xml = File.ReadAllText(@"FileSystems\FS2.xml");
            var fs = CreateFileSystem(xml);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CreateFs_DuplicateFolders_ThrowsException()
        {
            var xml = File.ReadAllText(@"FileSystems\FS3.xml");
            var fs = CreateFileSystem(xml);
        }

        [Test]
        public void CountFilesEmptyFs_ReturnsZero()
        {
            var xml = File.ReadAllText(@"FileSystems\FSEmpty.xml");
            var filesystem = CreateFileSystem(xml) as StubFileSystem<object>;

            var fileSelector = new StubFileSelector();
            fileSelector.Init(new StubFileSelectorSettings(new string[] { "" }));

            Assert.AreEqual(0, filesystem.CountFilesRecursively(filesystem.Root.Dir, fileSelector));
        }

        [Test]
        public void CountFilesNormalFs_ReturnsActualCount()
        {
            var xml = File.ReadAllText(@"FileSystems\FS1.xml");
            var filesystem = CreateFileSystem(xml) as StubFileSystem<object>;

            var fileSelector = new StubFileSelector();
            fileSelector.Init(new StubFileSelectorSettings(new string[] { "" }));

            Assert.AreEqual(6, filesystem.CountFilesRecursively(filesystem.Root.Dir, fileSelector));
        }

        private void AssertHasFiles(DirectoryItem<object> item, params string[] fileNames)
        {
            Assert.AreEqual(item.Files.Count, fileNames.Length, "Directory has different files count");

            var itemFileNames = item.Files.Select(f => f.File.FullName).ToArray();
            var fullNames = fileNames.Select(f => Path.Combine(item.Dir.FullName, f));

            Assert.True(itemFileNames.SequenceEqual(fullNames));
        }

        private void AssertHasNoFiles(DirectoryItem<object> item)
        {
            Assert.NotNull(item.Files);
            Assert.AreEqual(0, item.Files.Count);
        }

        private void AssertHasNoFolders(DirectoryItem<object> item)
        {
            Assert.NotNull(item.Childs);
            Assert.AreEqual(0, item.Childs.Count);
        }

        private void AssertHasFolders(DirectoryItem<object> parent, params string[] dirNames)
        {
            Assert.AreEqual(parent.Childs.Count, dirNames.Length, "Directory has different dubdirectories count");

            var subDirNames = parent.Childs.Select(sd => sd.Dir.FullName);

            Assert.True(subDirNames.SequenceEqual(dirNames));
        }

        private IFileSystem CreateFileSystem(string xml)
        {
            return new StubFileSystem<object>(xml);
        }
    }
}
