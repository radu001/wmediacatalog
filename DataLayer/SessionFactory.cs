using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using DataLayer.Caching;
using System;

namespace DataLayer
{
    public static class SessionFactory
    {
        public static ISession GetSession()
        {
            if (sessionFactory == null)
                Initialize();

            return sessionFactory.OpenSession();
        }

        public static void Initialize()
        {
            if (sessionFactory == null)
            {
                try
                {
                    sessionFactory = CreateSessionFactory();
                }
                catch (Exception ex)
                {
                    sessionFactory = null;
                    configurationCache.Cleanup(); // removed cache info since some errors with db connectivity
                    throw ex;
                }
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            if (configurationCache == null)
                configurationCache = new ConfigurationCache();
            
            Configuration config = null;

            if (!configurationCache.HasCachedConfiguration())
            {
                try
                {
                    config = new Configuration();
                    config.Configure();
                    config.AddAssembly(Assembly.GetExecutingAssembly());

                    configurationCache.SaveCachedConfiguration(config);
                }
                catch { }
            }
            else
            {
                config = configurationCache.GetCachedConfiguration();
            }

            return config.BuildSessionFactory();
        }

        private static ISessionFactory sessionFactory;
        private static IConfigurationCache configurationCache;
    }
}
