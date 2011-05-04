using System;
using NLog;

namespace Common
{
    public static class Logger
    {
        public static void Write(Exception ex)
        {
            if (log == null)
            {
                log = LogManager.GetCurrentClassLogger();
            }

            log.ErrorException("Exception", ex);
        }

        private static NLog.Logger log;
    }
}
