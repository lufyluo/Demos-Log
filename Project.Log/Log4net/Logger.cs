using System.Configuration;
using log4net;
using static System.Configuration.ConfigurationSettings;

namespace Project.Log.Log4net
{
    public class Logger : ILogger
    {
        private static ILog _logger;
        private static readonly object LogLock = 1;
        private static readonly string Appender;
        private Logger() { }

        static Logger()
        {
            Appender =ConfigurationManager.AppSettings["LogAppend"] ?? "ConsoleAppender";
        }

        public static ILog Log
        {
            get
            {

                if (_logger == null)
                {
                    lock (LogLock)
                    {
                        if (_logger == null)
                        {
                            _logger = LogManager.GetLogger(Appender);
                        }
                    }
                }
                return _logger;
            }
        }

       
    }
}
