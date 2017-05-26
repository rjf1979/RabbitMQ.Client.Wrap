using System;
using System.Collections.Generic;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class Queue : IQueue
    {
        /// <summary>
        /// 
        /// </summary>
        protected IConnectionFactory ConnectionFactory;
        /// <summary>
        /// 
        /// </summary>
        protected IConnection Connection;
        /// <summary>
        /// 
        /// </summary>
        protected IModel Channel;
        /// <summary>
        /// 
        /// </summary>
        protected IBasicProperties BasicProperties;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        protected Queue(Authorization authorization)
        {
            ConnectionFactory = new ConnectionFactory
            {
                UserName = authorization.UserName,
                Password = authorization.Password,
                VirtualHost = authorization.VHost,
                HostName = authorization.Host,
                Port = authorization.Port,
                AutomaticRecoveryEnabled = true
            };
            Init();
        }

        protected Action<string, Exception> ExceptionHandler;

        protected void EnterLogEvent(LogLevel logLevel, string message, Exception exception = null, params object[] args)
        {
            if (exception != null) ExceptionHandler?.Invoke(message, exception);
#if DEBUG
            Logger.Debug(message, exception, args);
#endif
#if TRACE
            Logger.Trace(message, exception, args);
#endif
            switch (logLevel)
            {
                case LogLevel.Info:
                    Logger.Info(message, exception, args);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(message, exception, args);
                    break;
                case LogLevel.Error:
                    Logger.Error(message, exception, args);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(message, exception, args);
                    break;
            }
        }

        protected void Init()
        {
            try
            {
                Connection = ConnectionFactory.CreateConnection();
                Channel = Connection.CreateModel();
                Connection.CallbackException += Connection_CallbackException;
                Channel.CallbackException += Channel_CallbackException;
            }
            catch (Exception exception)
            {
                var msg = "RabbitMQ Service Connection is exception";
                EnterLogEvent(LogLevel.Error, msg, exception);
                throw;
            }

        }

        #region -- Event
        private void Channel_CallbackException(object sender, Events.CallbackExceptionEventArgs e)
        {
            var msg = $"Channel is CallbackException,Time:{DateTime.Now}";
            EnterLogEvent(LogLevel.Error, msg, e.Exception);
        }

        private void Connection_CallbackException(object sender, Events.CallbackExceptionEventArgs e)
        {
            var msg = $"Connection is CallbackException,Time:{DateTime.Now}";
            EnterLogEvent(LogLevel.Error, msg, e.Exception);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="arguments"></param>
        public bool QueueDeclare(string queue, IDictionary<string, object> arguments = null)
        {
            try
            {
                var result = Channel.QueueDeclare(queue, true, false, false, arguments);
                return !string.IsNullOrEmpty(result.QueueName);
            }
            catch (Exception e)
            {
                var msg = $"QueueDeclare {queue} is exception {DateTime.Now}";
                EnterLogEvent(LogLevel.Error, msg, e);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        public void QueueBind(string exchange, string queue, string routingKey)
        {
            try
            {
                Channel.QueueBind(queue, exchange, routingKey);
            }
            catch (Exception e)
            {
                var msg = $"QueueBind exchange:{exchange} > queue:{queue} is exception {DateTime.Now}";
                EnterLogEvent(LogLevel.Error, msg, e);
                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        /// <param name="arguments"></param>
        public void ExchangeDeclare(string exchange, ExchangeType exchangeType, IDictionary<string, object> arguments = null)
        {
            try
            {
                Channel.ExchangeDeclare(exchange, exchangeType.ToString().ToLower(), true, false, arguments);
            }
            catch (Exception e)
            {
                var msg = $"ExchangeDeclare exchange:{exchange} is exception {DateTime.Now}";
                EnterLogEvent(LogLevel.Error, msg, e);
                throw;
            }

        }


        public void RegisterExceptionHandler(Action<string, Exception> exceptionHandler)
        {
            if (ExceptionHandler == null) ExceptionHandler = exceptionHandler;
        }
    }
}
