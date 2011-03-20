
using DataServices.NHibernate;
using Microsoft.Practices.Prism.Commands;
namespace Modules.DatabaseSettings.ViewModels
{
    public interface IConnectionSettingsViewModel
    {
        INHibernateConfig NHibernateConfig { get; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> TestConfigurationCommand { get; }
        DelegateCommand<object> SaveConfigurationCommand { get; }
    }
}
