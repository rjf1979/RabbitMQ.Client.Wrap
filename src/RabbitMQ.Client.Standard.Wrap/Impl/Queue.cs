using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    internal abstract class Queue : IQueue
    {
        /// <summary>
        /// 
        /// </summary>
        protected IConnection Connection
        {
            get
            {
                lock (this)
                {
                    if (!_connections.ContainsKey(Option.ConnectionString))
                        throw new ArgumentNullException("no find connection, Connection:" + Option.ConnectionString);
                    return _connections[Option.ConnectionString];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IModel Channel
        {
            get
            {
                lock (this)
                {
                    if (!_models.ContainsKey(Option.ConnectionString)) throw new ArgumentNullException("no find channel, Connection:" + Option.ConnectionString);
                    return _models[Option.ConnectionString];
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected IBasicProperties BasicProperties;

        private static readonly IDictionary<string, IModel> _models = new Dictionary<string, IModel>();
        private static readonly IDictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">rabbitMQ的配置信息</param>
        protected Queue(RabbitMqConfigOption option)
        {
            Option = option;
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(option.ConnectionString),
                AutomaticRecoveryEnabled = true
            };
            Init(connectionFactory);
            if (option.IsQueueDurable)
            {
                BasicProperties = Channel.CreateBasicProperties();
                BasicProperties.DeliveryMode = 2;
            }
        }

        protected Action<string, Exception> ExceptionHandler;

        protected void EnterLogEvent(LogLevel logLevel, string message, Exception exception = null, params object[] args)
        {
            if (exception != null) ExceptionHandler?.Invoke(message, exception);
#if DEBUG
            Option.Logger.LogDebug(exception, message, args);
#endif
#if TRACE
            Option.Logger.LogTrace(message, exception, args);
#endif
            switch (logLevel)
            {
                case LogLevel.Info:
                    Option.Logger.LogInformation(exception, message, args);
                    break;
                case LogLevel.Warn:
                    Option.Logger.LogWarning(exception, message, args);
                    break;
                case LogLevel.Error:
                    Option.Logger.LogError(exception, message, args);
                    break;
            }
        }

        protected void Init(IConnectionFactory connectionFactory)
        {
            try
            {
                lock (this)
                {
                    if (!_connections.ContainsKey(Option.ConnectionString))
                    {
                        var connection = connectionFactory.CreateConnection();
                        connection.CallbackException += Connection_CallbackException;
                        _connections.Add(Option.ConnectionString, connection);
                    }

                    if (!_models.ContainsKey(Option.ConnectionString))
                    {
                        var channel = Connection.CreateModel();
                        channel.CallbackException += Channel_CallbackException;
                        _models.Add(Option.ConnectionString, channel);
                    }
                }


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
                var result = Channel.QueueDeclare(queue, Option.IsQueueDurable, false, false, arguments);
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
                Channel.ExchangeDeclare(exchange, exchangeType.ToString().ToLower(), Option.IsExchangeDurable, false, arguments);
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

        public RabbitMqConfigOption Option { get; }
    }
}
