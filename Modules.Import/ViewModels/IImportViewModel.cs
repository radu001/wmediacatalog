using Microsoft.Practices.Prism.Commands;
using Modules.Import.Model;

namespace Modules.Import.ViewModels
{
    public interface IImportViewModel
    {
        DelegateCommand<object> ScanFilesCommand { get; }
        DelegateCommand<object> MatchAgainstDatabaseCommand { get; }

        IImportDataModel DataModel { get; }
    }
}
