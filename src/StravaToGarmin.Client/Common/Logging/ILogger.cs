using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaToGarmin.Client
{
    public interface ILogger
    {
        void Debug(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
        void Error(string msg, Exception ex);
        void Fatal(string msg);
        void Fatal(string msg, Exception ex);
    }

    internal class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(ILog log)
        {
            _log = log;
        }

        public void Debug(string msg)
        {
            _log.Debug(msg);
        }

        public void Info(string msg)
        {
            _log.Info(msg);
        }

        public void Warn(string msg)
        {
            _log.Warn(msg);
        }

        public void Error(string msg)
        {
            _log.Error(msg);
        }

        public void Error(string msg, Exception ex)
        {
            _log.Error(msg, ex);
        }

        public void Fatal(string msg)
        {
            _log.Fatal(msg);
        }

        public void Fatal(string msg, Exception ex)
        {
            _log.Fatal(msg, ex);
        }

    }
}
