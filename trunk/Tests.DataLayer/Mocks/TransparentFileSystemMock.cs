using System.Collections.Generic;
using MediaCatalog.Tests.Extensions;
using Modules.Import.Model;
using Modules.Import.Services.Utils;
using Modules.Import.Services.Utils.FileSystem;

namespace MediaCatalog.Tests.Mocks
{
    public class TransparentFileSystemMock : IScanner, IFileSystem
    {
        public TransparentFileSystemMock(string xml)
        {
            fs = new StubFileSystem<FileTagCollection>(xml, new TagDataResolver());
        }

        public IDictionary<Dir, IEnumerable<FsFile>> GetDirectories()
        {
            return fs.GetDirectories();
        }

        #region IScanner Members

        public FileTagCollection GetTags(string filePath)
        {
            var fileData = fs.GetFileData(filePath);
            return fileData;
        }

        #endregion

        #region IFileSystem Members

        public int CountFilesRecursively(Dir dir, IFileSelector fileSelector)
        {
            return fs.CountFilesRecursively(dir, fileSelector);
        }

        public IEnumerable<FsFile> GetFiles(Dir dir, IFileSelector fileSelector)
        {
            return fs.GetFiles(dir, fileSelector);
        }

        public IEnumerable<Dir> GetSubDirectories(Dir dir)
        {
            return fs.GetSubDirectories(dir);
        }

        #endregion

        private StubFileSystem<FileTagCollection> fs;
    }
}
