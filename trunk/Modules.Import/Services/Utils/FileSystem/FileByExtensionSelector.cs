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

        public IEnumerable<FileInfo> SelectFiles(DirectoryInfo dir)
        {
            return dir.GetFiles(Settings.FileMasks.First()); // temporary dirty hack
        }

        #endregion
    }
}
