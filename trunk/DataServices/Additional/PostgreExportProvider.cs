using System;
using System.IO;
using System.Linq;
using Common;
using Common.Entities;
using DataServices.NHibernate;

namespace DataServices.Additional
{
    public class PostgreExportProvider : IExportProvider
    {
        private static string PgDumpFileName = "pg_dump.exe";

        #region IExportProvider Members

        public ExportProviderSettings Settings { get; set; }

        public TextResult ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(Settings.UserName) || String.IsNullOrWhiteSpace(Settings.Password))
                return new TextResult()
                {
                    Success = false,
                    Message = "Please provide non empty login/password"
                };

            if (!Directory.Exists(Settings.ProviderPath))
                return new TextResult()
                {
                    Success = false,
                    Message = "Please provide valid provider path"
                };

            //validate pg_dump executable
            FileInfo dumpExecutable = null;
            try
            {
                dumpExecutable = new FileInfo(Path.Combine(Settings.ProviderPath, PgDumpFileName));

                if (!dumpExecutable.Exists)
                {
                    return new TextResult()
                    {
                        Success = false,
                        Message = "Can't locate pg_dump executable using given provider path"
                    };
                }

            }
            catch (Exception ex)
            {
                return new TextResult()
                {
                    Success = false,
                    Message = "Can't locate pg_dump executable using given provider path"
                };
            }


            if (!Directory.Exists(Settings.ExportPath))
                return new TextResult()
                {
                    Success = false,
                    Message = "Please provide valid export path"
                };

            if (String.IsNullOrWhiteSpace(Settings.ExportFileName))
                return new TextResult()
                {
                    Success = false,
                    Message = "Please provide name for export file"
                };

            try
            {
                string exportFileFullPath = Path.Combine(Settings.ExportPath, Settings.ExportFileName);

                try
                {
                    FileStream fs = File.Create(exportFileFullPath);
                    fs.Write(new byte[] { 0x01 }, 0, 1);
                    fs.Close();

                    File.Delete(exportFileFullPath);
                }
                catch (Exception ex)
                {
                    return new TextResult()
                    {
                        Success = false,
                        Message = String.Format("Can't write to file {0}. Check" + 
                                                    "filesystem permissions or choose other file name or path", exportFileFullPath)
                    };
                }
            }
            catch
            {
                return new TextResult()
                {
                    Success = false,
                    Message = "Illegal export path and file name. Choose other export path and file name"
                };
            }

            return new TextResult()
            {
                Success = true
            };
        }

        public void BeginExport(Action<TextResult> finishAction)
        {
            try
            {
                string dbName = GetDatabaseName();

                //TODO: setup PGPASSWORD variable and run pg_dump

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);

                finishAction(new TextResult()
                {
                    Success = false,
                    Message = "Error while exporting database occured. Please see log for details"
                });
            }
        }

        #endregion

        #region Private methods

        private string GetDatabaseName()
        {
            //TODO locate nhibernate config and grab database name from it
            INHibernateConfig config = new NHibernateConfigModel();
            if (!config.Load())
                throw new Exception("Can't load NHibernate configuration file");

            var connectionString = config.Properties.Where(p => p.Name == "connection.connection_string").FirstOrDefault();
            var dbName = connectionString.Values.Where(cp => cp.Name == "Database").FirstOrDefault();

            return dbName.Value;
        }

        #endregion
    }
}
