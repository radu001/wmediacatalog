using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;

namespace DataLayer.Caching
{
    public interface IConfigurationCache
    {
        bool HasCachedConfiguration();

        void SaveCachedConfiguration(Configuration config);
        Configuration GetCachedConfiguration();
        void Cleanup();
    }
}
