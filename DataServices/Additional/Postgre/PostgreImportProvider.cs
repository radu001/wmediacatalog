using System;
using System.Diagnostics;
using System.IO;
using Common;
using Common.Entities;
using DataServices.Additional.Base;

namespace DataServices.Additional.Postgre
{
    public class PostgreImportProvider : PostgreProviderBase, IImportProvider
    {
        private static readonly string PsqlName = "psql.exe";

        #region IImportProvider Members

        public ImportProviderSettings Settings { get; set; }

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

            if (!File.Exists(Settings.BackupFullName))
                return new TextResult()
                {
                    Success = false,
                    Message = "Backup file doesn't exist"
                };

            return new TextResult()
            {
                Success = true
            };
        }

        public TextResult Import(ImportProviderSettings settings)
        {
            try
            {
                string dbName = GetDatabaseName();

                var args = String.Format("-f \"{0}\" -U {1} {2}", Settings.BackupFullName, Settings.UserName, dbName);

                var process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(Settings.ProviderPath, PsqlName),
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
                        Message = "Database dump has been successfully restored."
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
                    Message = "Error while importing database. Please see log for details"
                };
            }
        }

        #endregion
    }
}
