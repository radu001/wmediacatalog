using System.Collections.Generic;
using System.IO;
using Modules.Import.Services.Utils.FileSystem;

namespace MediaCatalog.Tests.Mocks
{
    public class StubFileSelector : IFileSelector
    {
        #region IFileSelector Members

        public void Init(IFileSelectorSettings settings)
        {
        }

        public IEnumerable<FileInfo> SelectFiles(DirectoryInfo dir)
        {
            return null;
        }

        #endregion
    }
}
