using System;

namespace log4net
{
    internal interface ILog
    {
        void Info(string msg, Exception ex = null);
        void InfoFormat(string msg, params object[] parameters);
        void Warn(string msg, Exception ex = null);
        void WarnFormat(string msg, params object[] parameters);
        void Error(string msg, Exception ex = null);
        void ErrorFormat(string msg, params object[] parameters);
    }

    internal class LogImpl : ILog
    {
        public void Info(string msg, Exception ex = null)
        {
        }

        public void InfoFormat(string msg, params object[] parameters)
        {
        }

        public void Warn(string msg, Exception ex = null)
        {
        }

        public void WarnFormat(string msg, params object[] parameters)
        {
        }

        public void Error(string msg, Exception ex = null)
        {
        }

        public void ErrorFormat(string msg, params object[] parameters)
        {
        }
    }
}
