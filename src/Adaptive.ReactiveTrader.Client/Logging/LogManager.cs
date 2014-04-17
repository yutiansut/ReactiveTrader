using System;

namespace log4net
{
    internal class LogManager
    {
        public  static ILog GetLogger(Type type)
        {
            return new LogImpl();
        }
    }
}
