using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class Queue : IQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="option">rabbitMQ的配置信息</param>
        protected Queue(ILogger logger,RabbitMQConfigOption option)
        {
            Logger = logger;
            Option = option;
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(Option.Connection),
                AutomaticRecoveryEnabled = true
            };
            Init(connectionFactory);
            //if (option.IsQueueDurable)
            //{
                BasicProperties = Channel.CreateBasicProperties();
                BasicProperties.DeliveryMode = 2;
            //}
        }

        protected ILogger Logger { get; set; }

        public RabbitMQConfigOption Option { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IConnection Connection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected IModel Channel { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        protected IBasicProperties BasicProperties;


        protected Action<string, Exception> ExceptionHandler;

        protected void EnterLogEvent(string message, Exception exception = null)
        {
            if (exception != null) Logger.LogError(exception, message);
            else Logger.LogInformation(message);
        }

        protected void Init(IConnectionFactory connectionFactory)
        {
            try
            {
                Connection = connectionFactory.CreateConnection();
                Connection.CallbackException += Connection_CallbackException;
            }
            catch (Exception exception)
            {
                var msg = "RabbitMQ Service CreateConnection is exception";
                EnterLogEvent(msg, exception);
                throw;
            }
            try
            {
                Channel = Connection.CreateModel();
                Channel.CallbackException += Channel_CallbackException;
            }
            catch (Exception exception)
            {
                var msg = "RabbitMQ Service CreateModel is exception";
                EnterLogEvent(msg, exception);
                throw;
            }
        }

        #region -- Event
        private void Channel_CallbackException(object sender, Events.CallbackExceptionEventArgs e)
        {
            var msg = $"Channel is CallbackException,Time:{DateTime.Now}";
            EnterLogEvent(msg, e.Exception);
        }

        private void Connection_CallbackException(object sender, Events.CallbackExceptionEventArgs e)
        {
            var msg = $"Connection is CallbackException,Time:{DateTime.Now}";
            EnterLogEvent(msg, e.Exception);
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
                EnterLogEvent(msg, e);
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
                EnterLogEvent(msg, e);
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
                EnterLogEvent(msg, e);
                throw;
            }

        }

        public void RegisterExceptionHandler(Action<string, Exception> exceptionHandler)
        {
            if (ExceptionHandler == null) ExceptionHandler = exceptionHandler;
        }


    }
}
