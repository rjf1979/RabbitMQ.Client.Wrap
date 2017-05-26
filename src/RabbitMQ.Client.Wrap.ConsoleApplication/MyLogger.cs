using System;

namespace RabbitMQ.Client.Wrap.ConsoleApplication
{
    class MyLogger:RabbitMQ.Client.Wrap.Interface.ILog
    {
        public void Info(string message, Exception exception = null, params object[] args)
        {
            //实现你的记录日志代码
        }

        public void Warn(string message, Exception exception = null, params object[] args)
        {
            //实现你的记录日志代码
        }

        public void Error(string message, Exception exception = null, params object[] args)
        {
            //实现你的记录日志代码
        }

        public void Fatal(string message, Exception exception = null, params object[] args)
        {
            //实现你的记录日志代码
        }

        public void Debug(string message, Exception exception = null, params object[] args)
        {
            //实现你的记录日志代码
            //此日志会在#if DEBUG下会运行
        }

        public void Trace(string message, Exception exception = null, params object[] args)
        {
            //实现你的记录日志代码
            //此日志会在#if TRACE下会运行
        }
    }
}
