
using System;
using System.IO;
namespace Modules.DatabaseSettings.Utils.Psql
{
    /// <summary>
    /// Provides mechanism of location of psql executable on filesystem
    /// </summary>
    public interface IPsqlLocator
    {
        PsqlSearchStatusEnum SearchStatus { get; }
        int TotalDirs { get; }
        int ScannedDirs { get; }
        string SearchPattern { get; }
        string InitialPath { get; set; }
        string CurrentPath { get; }

        void BeginLocate(Action<FileInfo> completeAction);
        void CancelLocate();
    }
}
