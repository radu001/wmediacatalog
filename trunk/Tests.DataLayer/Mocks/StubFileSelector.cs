using System.Collections.Generic;
using Modules.Import.Services.Utils.FileSystem;
using System.Linq;

namespace MediaCatalog.Tests.Mocks
{
    public class StubFileSelector : IFileSelector
    {
        public StubFileSelector(IDictionary<Dir, IEnumerable<FsFile>> cache)
        {
            filesCache = new Dictionary<string, List<FsFile>>();

            if (cache != null)
            {
                foreach (var kvp in cache)
                {
                    filesCache[kvp.Key.FullName] = kvp.Value.ToList();
                }
            }
        }

        #region IFileSelector Members

        public void Init(IFileSelectorSettings settings)
        {
        }

        public IEnumerable<FsFile> SelectFiles(string dirPath)
        {
            if (!filesCache.ContainsKey(dirPath))
                return new FsFile[] { };

            return filesCache[dirPath];
        }

        #endregion

        private Dictionary<string, List<FsFile>> filesCache;
    }
}
