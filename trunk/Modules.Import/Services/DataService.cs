using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BusinessObjects;
using Common;
using DataServices;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.Services.Utils;

namespace Modules.Import.Services
{
    public class DataService : IDataService
    {
        public DataService(IUnityContainer container, IScanner scanner, IFileSystem fileSystem, ITagsAccumulator tagsAccumulator)
        {
            this.container = container;
            this.scanner = scanner;
            this.fileSystem = fileSystem;
            this.tagsAccumulator = tagsAccumulator;
        }

        public IEnumerable<Artist> BeginScan(ScanSettings settings)
        {
            DirectoryInfo dir = new DirectoryInfo(settings.ScanPath);
            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();
            stack.Push(dir);

            if (settings.BeforeScan != null)
            {
                int totalFiles = fileSystem.CountFilesRecursively(dir, settings.FileMask);
                settings.BeforeScan(totalFiles);
            }

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
                    try
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

        #endregion
    }
}
