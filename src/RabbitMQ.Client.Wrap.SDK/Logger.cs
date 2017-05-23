using System;
using System.Collections.Generic;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger
    {
        private static readonly IList<ILog> _loggers = new List<ILog>();

        /// <summary>
        /// 添加一个日志记录器
        /// </summary>
        /// <param name="log"></param>
        public static void AddLogger(ILog log)
        {
            _loggers.Add(log);
        }

        public static void Info(string message, Exception exception = null, params object[] args)
        {
            throw new NotImplementedException();
        }

        public static void Warn(string message, Exception exception = null, params object[] args)
        {
            throw new NotImplementedException();
        }

        public static void Error(string message, Exception exception = null, params object[] args)
        {
            //throw new NotImplementedException();
        }

        public static void Fatal(string message, Exception exception = null, params object[] args)
        {
            throw new NotImplementedException();
        }

        public static void Debug(string message, Exception exception = null, params object[] args)
        {
            throw new NotImplementedException();
        }

        public static void Trace(string message, Exception exception = null, params object[] args)
        {
            throw new NotImplementedException();
        }

        public static void Console(string message, Exception exception = null)
        {
            System.Console.WriteLine(message);
            System.Console.WriteLine(exception);
            System.Console.WriteLine("================================================================================");
        }
    }
}
