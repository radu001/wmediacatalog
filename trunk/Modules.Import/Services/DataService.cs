using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BusinessObjects;
using Common;
using DataServices;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services.Utils;
using Modules.Import.Services.Utils.FileSystem;

namespace Modules.Import.Services
{
    public class DataService : IDataService
    {
        public DataService(IUnityContainer container, IScanner scanner, IFileSystem fileSystem, ITagsAccumulator tagsAccumulator, IFileSelector fileSelector)
        {
            this.container = container;
            this.scanner = scanner;
            this.fileSystem = fileSystem;
            this.tagsAccumulator = tagsAccumulator;
            this.fileSelector = fileSelector;
        }

        public IEnumerable<Artist> BeginScan(ScanSettings settings)
        {
            fileSelector.Init(settings);

            var dir = new Dir(settings.ScanPath);
            var stack = new Stack<Dir>();
            stack.Push(dir);

            if (settings.BeforeScan != null)
            {
                int totalFiles = fileSystem.CountFilesRecursively(dir, fileSelector);
                settings.BeforeScan(totalFiles);
            }

            while (stack.Count > 0 && !settings.Stop)
            {
                if (settings.Pause)
                {
                    Thread.Sleep(500);
                    continue;
                }

                var currentDir = stack.Pop();
                var subDirectories = fileSystem.GetSubDirectories(currentDir);
                foreach (var subDir in subDirectories)
                    stack.Push(subDir);

                if (settings.BeginDirectoryScan != null)
                {
                    settings.BeginDirectoryScan(currentDir.FullName);
                }

                var files = fileSystem.GetFiles(currentDir, fileSelector);
                foreach (var f in files)
                {
                    try
                    {
                        var tags = scanner.GetTags(f.FullName);

                        if (tags.Count() > 0)
                        {
                            if (settings.BeginFileScan != null)
                            {
                                settings.BeginFileScan(f.FullName);
                            }

                            tagsAccumulator.AccumulateTags(tags);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(ex);
                    }
                }
            }

            return tagsAccumulator.GetAccumulatedResult();
        }

        public bool BulkImportData(IEnumerable<Artist> artists)
        {
            DataProvider provider = new DataProvider();
            return provider.BulkImportData(artists);
        }

        #region Private fields

        private readonly IUnityContainer container;
        private readonly IScanner scanner;
        private readonly IFileSystem fileSystem;
        private readonly ITagsAccumulator tagsAccumulator;
        private readonly IFileSelector fileSelector;

        #endregion
    }
}
