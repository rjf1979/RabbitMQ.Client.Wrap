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
            foreach (var logger in _loggers)
            {
                logger.Info(message, exception, args);
            }
        }

        public static void Warn(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Warn(message, exception, args);
            }
        }

        public static void Error(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(message, exception, args);
            }
        }

        public static void Fatal(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal(message, exception, args);
            }
        }

        public static void Debug(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Debug(message, exception, args);
            }
        }

        public static void Trace(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Trace(message, exception, args);
            }
        }

        public static void Console(string message, Exception exception = null)
        {
            System.Console.WriteLine(message);
            System.Console.WriteLine(exception);
            System.Console.WriteLine("================================================================================");
        }
    }
}
