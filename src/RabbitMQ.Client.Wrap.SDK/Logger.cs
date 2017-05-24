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

        internal static void Info(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Info(message, exception, args);
            }
        }

        internal static void Warn(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Warn(message, exception, args);
            }
        }

        internal static void Error(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(message, exception, args);
            }
        }

        internal static void Fatal(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal(message, exception, args);
            }
        }

        internal static void Debug(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Debug(message, exception, args);
            }
        }

        internal static void Trace(string message, Exception exception = null, params object[] args)
        {
            foreach (var logger in _loggers)
            {
                logger.Trace(message, exception, args);
            }
        }
    }
}
