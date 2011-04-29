using System.Collections.Generic;
using System.IO;
using MediaCatalog.Tests.Extensions;
using Modules.Import.Model;
using Modules.Import.Services.Utils;
using Modules.Import.Services.Utils.FileSystem;

namespace MediaCatalog.Tests.Mocks
{
    public class StubTagsScanner : IScanner, IFileSystem
    {
        public StubTagsScanner(string xml)
        {
            fs = new StubFileSystem<FileTagCollection>(xml, new TagDataResolver());
        }

        #region IScanner Members

        public FileTagCollection GetTags(string filePath)
        {
            var fileData = fs.GetFileData(filePath);
            return fileData;
        }

        #endregion

        #region IFileSystem Members

        public int CountFilesRecursively(DirectoryInfo dir, IFileSelector fileSelector)
        {
            return fs.CountFilesRecursively(dir, fileSelector);
        }

        public IEnumerable<FileInfo> GetFiles(DirectoryInfo dir, IFileSelector fileSelector)
        {
            return fs.GetFiles(dir, fileSelector);
        }

        public IEnumerable<DirectoryInfo> GetSubDirectories(DirectoryInfo dir)
        {
            return fs.GetSubDirectories(dir);
        }

        #endregion

        private StubFileSystem<FileTagCollection> fs;
    }
}
