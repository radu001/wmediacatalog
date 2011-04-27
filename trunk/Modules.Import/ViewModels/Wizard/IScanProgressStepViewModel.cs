
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public interface IScanProgressStepViewModel : IWizardViewModel
    {
        string ScanPath { get; }
        bool IsScanning { get; }
        bool IsPaused { get; }
        bool IsCompleted { get; }

        //UI
        int ScanFilesCount { get; }
        float MinProgress { get; }
        float MaxProgress { get; }
        float CurrentProgress { get; }
        StringBuilder Log { get; }

        DelegateCommand<object> BeginScanCommand { get; }
        DelegateCommand<object> PauseScanCommand { get; }
        DelegateCommand<object> CancelScanCommand { get; }
    }
}
