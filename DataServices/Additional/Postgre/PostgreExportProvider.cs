using System;
using System.Diagnostics;
using System.IO;
using Common;
using Common.Entities;
using DataServices.Additional.Base;

namespace DataServices.Additional.Postgre
{
    public class PostgreExportProvider : PostgreProviderBase, IExportProvider
    {
        private static readonly string PgDumpFileName = "pg_dump.exe";

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

        public TextResult Export()
        {
            try
            {
                string dbName = GetDatabaseName();

                var args = String.Format("-O -c -f \"{2}\" -U {0} {1} ", Settings.UserName, dbName,
                    Path.Combine(Settings.ExportPath, Settings.ExportFileName));

                var process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(Settings.ProviderPath, PgDumpFileName),
                    Arguments = args,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                process.StartInfo.EnvironmentVariables.Add("PGPASSWORD", Settings.Password);

                process.Start();
                process.WaitForExit();

                var errorMessage = process.StandardError.ReadToEnd();
                if (String.IsNullOrEmpty(errorMessage))
                {
                    return new TextResult()
                    {
                        Success = true,
                        Message = "Database dump has been successfully created."
                    };
                }
                else
                {
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);

                return new TextResult()
                {
                    Success = false,
                    Message = "Error while exporting database. Please see log for details"
                };
            }
        }

        #endregion
    }
}
