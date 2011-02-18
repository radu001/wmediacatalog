
using Microsoft.Practices.Composite.Presentation.Commands;
using Modules.DatabaseSettings.Data;
namespace Modules.DatabaseSettings.ViewModels
{
    public interface IConnectionSettingsViewModel
    {
        INHibernateConfig NHibernateConfig { get; }
        
        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> TestConfigurationCommand { get; }
        DelegateCommand<object> SaveConfigurationCommand { get; }
        DelegateCommand<object> AdvancedSettingsCommand { get; }
    }
}
