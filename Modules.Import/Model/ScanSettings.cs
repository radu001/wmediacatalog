using System;

namespace Modules.Import.Model
{
    public class ScanSettings
    {
        public bool Stop { get; set; }
        public bool Pause { get; set; }
        public string ScanPath { get; set; }
        public string FileMask { get; set; }
        public Action<int> BeforeScan { get; set; }
        public Action<string> BeginDirectoryScan { get; set; }
        public Action<string> BeginFileScan { get; set; }
    }
}
