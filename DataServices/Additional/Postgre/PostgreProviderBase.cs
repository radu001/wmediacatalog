
using System;
using System.Linq;
using DataServices.NHibernate;
namespace DataServices.Additional.Postgre
{
    public abstract class PostgreProviderBase
    {
        protected string GetDatabaseName()
        {
            INHibernateConfig config = new NHibernateConfigModel();
            if (!config.Load())
                throw new Exception("Can't load NHibernate configuration file");

            var connectionString = config.GetConnectionString();
            var dbName = connectionString.Values.Where(cp => cp.Name == "Database").FirstOrDefault();

            return dbName.Value;
        }
    }
}
