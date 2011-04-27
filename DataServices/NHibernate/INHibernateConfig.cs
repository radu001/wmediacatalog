
using System.Collections.ObjectModel;
namespace DataServices.NHibernate
{
    public interface INHibernateConfig
    {
        ObservableCollection<ConfigurationProperty> Properties { get; }
        string FileName { get; }

        bool Load();
        bool Save(string fileName);
        ConfigurationProperty GetConnectionString();
    }
}
