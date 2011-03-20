
using Microsoft.Practices.Prism.Commands;
namespace Modules.DatabaseSettings.ViewModels
{
    public interface IDatabaseToolsViewModel
    {
        string UserName { get; set; }
        string Password { get; set; }
        string ProviderPath { get; set; }
        string ExportPath { get; set; }
        string ExportFileName { get; set; }

        DelegateCommand<object> ExportDatabaseCommand { get; }
        DelegateCommand<object> SelectProviderPathCommand { get; }
        DelegateCommand<object> SelectExportPathCommand { get; }
    }
}
