using log4net;

namespace Project.Log.Log4net
{
    public class Logger : ILogger
    {
        private static ILog _logger;
        private static readonly object LogLock = 1;
        private Logger() { }

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
                            _logger = LogManager.GetLogger("ElasticsearchAppender");
                        }
                    }
                }
                return _logger;
            }
        }

       
    }
}
