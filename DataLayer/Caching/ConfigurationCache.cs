
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Common;
using NHibernate.Cfg;
namespace DataLayer.Caching
{
    public class ConfigurationCache : IConfigurationCache
    {
        #region IConfigurationCache Members

        public bool HasCachedConfiguration()
        {
            ModuleInfo info = UpdateCachedModuleInfo();

            if (!info.DiffersFromCached)
                return true;

            return false;
        }

        public Configuration GetCachedConfiguration()
        {
            if (!File.Exists(NhibernateCacheConfig))
                throw new OperationCanceledException("Cached config file not found");

            Configuration config = null;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var fileStream = new FileStream(NhibernateCacheConfig, FileMode.Open))
            {
                config = formatter.Deserialize(fileStream) as Configuration;
            }

            return config;
        }

        public void SaveCachedConfiguration(Configuration config)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var fileStream = new FileStream(NhibernateCacheConfig, FileMode.Create))
            {
                formatter.Serialize(fileStream, config);
            }
        }

        public void Cleanup()
        {
            if (File.Exists(NHibernateCacheModuleVersion))
                File.Delete(NHibernateCacheModuleVersion);

            if (File.Exists(NhibernateCacheConfig))
                File.Delete(NhibernateCacheConfig);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates (saves or creates) current module info into filesystem cache. If there exists cached module info
        /// and current module info isn't changed if compared to cached one method updates nothing
        /// </summary>
        /// <returns>Module information from cache</returns>
        private ModuleInfo UpdateCachedModuleInfo()
        {
            ModuleInfo result = null;

            BinaryFormatter formatter = new BinaryFormatter();

            Assembly current = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = current.GetName();

            Version currentVersion = assemblyName.Version;


            if (!File.Exists(NHibernateCacheModuleVersion))
            {
                using (FileStream fs = File.Create(NHibernateCacheModuleVersion))
                {
                    formatter.Serialize(fs, currentVersion);
                }

                result = new ModuleInfo()
                {
                    Version = currentVersion,
                    DiffersFromCached = true
                };
            }
            else
            {
                Version cachedVersion = null;

                using (FileStream fs = File.OpenRead(NHibernateCacheModuleVersion))
                {
                    try
                    {
                        cachedVersion = (Version)formatter.Deserialize(fs);
                    }
                    catch(Exception ex)
                    {
                        Logger.Write(ex);
                    }
                }

                if (cachedVersion != currentVersion) // update cached guid
                {
                    using (FileStream fs = File.Create(NHibernateCacheModuleVersion))
                    {
                        formatter.Serialize(fs, currentVersion);
                    }
                }

                result = new ModuleInfo()
                {
                    Version = currentVersion,
                    DiffersFromCached = (cachedVersion != currentVersion)
                };

            }

            return result;
        }

        #endregion

        private static string NhibernateCacheConfig = "nhibernate.cfg.cache";
        private static string NHibernateCacheModuleVersion = "nhibernate.modver.cache";
    }

    /// <summary>
    /// Information about module
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        /// Gets or sets module Guid
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets value indicating that current module has changed if compared to previous cached version.
        /// Comparison is made by Guid of current module and cached one
        /// </summary>
        public bool DiffersFromCached { get; set; }
    }
}
