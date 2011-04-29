using System;
using Modules.Import.Services.Utils.FileSystem;

namespace Modules.Import.Model
{
    public class ScanSettings : IFileSelectorSettings
    {
        public bool Stop { get; set; }
        public bool Pause { get; set; }
        public string ScanPath { get; set; }
        public Action<int> BeforeScan { get; set; }
        public Action<string> BeginDirectoryScan { get; set; }
        public Action<string> BeginFileScan { get; set; }

        #region IFileSelectorSettings Members

        public string[] FileMasks { get; set; }

        #endregion
    }
}
