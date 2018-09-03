using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RabbitMQ.Client.Standard.Wrap
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger
    {
        private static readonly IList<ILogger> _loggers = new List<ILogger>();

        /// <summary>
        /// 注册一个日志记录器
        /// </summary>
        /// <param name="logger"></param>
        public static void RegisiterLogger(ILogger logger)
        {
            _loggers.Add(logger);
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
