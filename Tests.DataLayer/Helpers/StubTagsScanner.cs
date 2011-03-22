using System.Collections.Generic;
using Modules.Import.Model;
using Modules.Import.Services.Utils;

namespace MediaCatalog.Tests.Helpers
{
    public class StubTagsScanner : IScanner
    {
        #region IScanner Members

        public IEnumerable<FileTag> GetTags(string filePath)
        {
            return null;
        }

        #endregion
    }
}
