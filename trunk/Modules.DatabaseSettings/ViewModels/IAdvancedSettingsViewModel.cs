
using Microsoft.Practices.Composite.Presentation.Commands;
using Modules.DatabaseSettings.Data;
using Modules.DatabaseSettings.Utils.Psql;
namespace Modules.DatabaseSettings.ViewModels
{
    public interface IAdvancedSettingsViewModel
    {
        DatabaseDeployInfo DatabaseDeployInfo { get; }
        IPsqlLocator PsqlLocator { get; }
        bool PsqlLocatorDetailsVisible { get; set; }

        DelegateCommand<object> LocatePsqlCommand { get; }
        DelegateCommand<object> CancelLocatePsqlCommand { get; }
        DelegateCommand<object> SetPsqlSearchPathCommand { get; }

        DelegateCommand<object> ValidateSchemaCommand { get; }
        DelegateCommand<object> DeploySchemaCommand { get; }
        DelegateCommand<object> BasicSettingsCommand { get; }
    }
}
