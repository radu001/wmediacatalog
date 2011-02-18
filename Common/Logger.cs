using System;
using log4net;
using log4net.Config;

namespace Common
{
    public static class Logger
    {
        public static void Write(Exception ex)
        {
            if (log == null)
            {
                XmlConfigurator.Configure();
                log = LogManager.GetLogger("Default");
            }

            log.Error("Exception", ex);
        }

        private static ILog log;
    }
}
