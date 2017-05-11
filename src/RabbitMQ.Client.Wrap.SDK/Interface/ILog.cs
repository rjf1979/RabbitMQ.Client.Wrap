using System;

namespace RabbitMQ.Client.Wrap.Interface
{
    public interface ILog
    {
        void Info(string message, Exception exception = null, params object[] args);
        void Warn(string message, Exception exception = null, params object[] args);
        void Error(string message, Exception exception = null, params object[] args);
        void Fatal(string message, Exception exception = null, params object[] args);
        void Debug(string message, Exception exception = null, params object[] args);
        void Trace(string message, Exception exception = null, params object[] args);
    }
}
