
using System.Collections.ObjectModel;
namespace Modules.DatabaseSettings.Data
{
    public interface INHibernateConfig
    {
        ObservableCollection<ConfigurationProperty> Properties { get; }

        bool Load(string fileName);
        bool Save(string fileName);
    }
}
