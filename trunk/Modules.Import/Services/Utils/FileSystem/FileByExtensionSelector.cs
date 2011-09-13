using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Modules.Import.Services.Utils.FileSystem
{
    public class FileByExtensionSelector : IFileSelector
    {
        public IFileSelectorSettings Settings { get; private set; }

        public FileByExtensionSelector() { }

        #region IFileSelector Members

        public void Init(IFileSelectorSettings settings)
        {
            if (settings == null)
                throw new NullReferenceException("Illegal null-reference settings");
            Settings = settings;
        }

        public IEnumerable<FsFile> SelectFiles(string dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            IEnumerable<FsFile> result = null;

            try
            {
                result = dir.GetFiles(Settings.FileMasks.First()).
                    Select<FileInfo, FsFile>(f => new FsFile(f.FullName)); // temporary dirty hack
            }
            catch (Exception ex)
            {
                result = new FsFile[] { };
            }

            return result;
        }

        #endregion
    }
}
