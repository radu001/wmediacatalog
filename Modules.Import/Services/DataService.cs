using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BusinessObjects;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services.Utils;

namespace Modules.Import.Services
{
    public class DataService : IDataService
    {
        public DataService(IUnityContainer container)
        {
            this.container = container;
        }

        public IEnumerable<Artist> BeginScan(ScanSettings settings)
        {
            List<Artist> result = new List<Artist>();

            IScanner scanner = container.Resolve<IScanner>();
            IFileSystem fileSystem = container.Resolve<IFileSystem>();
            ITagsAccumulator tagsAccumulator = container.Resolve<ITagsAccumulator>();

            DirectoryInfo dir = new DirectoryInfo(settings.ScanPath);
            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();
            stack.Push(dir);

            while (stack.Count > 0 && !settings.Stop)
            {
                if (settings.Pause)
                {
                    Thread.Sleep(500);
                    continue;
                }

                DirectoryInfo currentDir = stack.Pop();
                var subDirectories = fileSystem.GetSubDirectories(currentDir);
                foreach (var subDir in subDirectories)
                    stack.Push(subDir);

                if (settings.BeginDirectoryScan != null)
                {
                    settings.BeginDirectoryScan(currentDir.FullName);
                }

                var files = fileSystem.GetFiles(currentDir, settings.FileMask);
                foreach (var f in files)
                {
                    var tags = scanner.GetTags(f.FullName);

                    if (tags.Count() > 0)
                    {
                        if (settings.BeginFileScan != null)
                        {
                            settings.BeginFileScan(f.Name);
                        }

                        tagsAccumulator.AccumulateTags(tags);
                    }
                }
            }

            return result;
        }

        #region Private fields

        private IUnityContainer container;

        #endregion
    }
}
