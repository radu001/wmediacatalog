
namespace Modules.Import.ViewModels
{
    public interface IImportProgressViewModel
    {
        int ScanFilesCount { get; set; }
        int ScannedFilesCount { get; }

        void Init();
        void NotifyFileScanned();
    }
}
