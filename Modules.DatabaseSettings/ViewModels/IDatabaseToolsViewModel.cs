
using Microsoft.Practices.Composite.Presentation.Commands;
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
    }
}
