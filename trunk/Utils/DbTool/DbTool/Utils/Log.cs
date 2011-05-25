using System;
using System.IO;
using NLog;

namespace DbTool.Utils
{
    public class Log : ILog
    {
        #region ILog Members

        public void Write(Exception ex)
        {
            if (log == null)
            {
                log = LogManager.GetCurrentClassLogger();
            }

            log.ErrorException("Exception", ex);
        }

        public string[] GetAllLog()
        {
            return File.ReadAllLines("DbDeploy.log");
        }

        #endregion

        private static Logger log;
    }
}
