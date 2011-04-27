
using System.Collections.Generic;
using BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public interface IBulkImportViewModel : IWizardViewModel
    {
        bool ImportCompleted { get; }
        IEnumerable<Artist> Artists { get; }

        DelegateCommand<object> BeginImportCommand { get; }
        DelegateCommand<object> ViewLoadedCommand { get; }
    }
}
