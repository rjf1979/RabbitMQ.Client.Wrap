using System;
using System.Collections.Generic;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="message"></param>
    ///// <param name="exception"></param>
    //public delegate void ExceptionHandler(string message, Exception exception);

    /// <summary>
    /// 
    /// </summary>
    internal abstract class Queue:IQueue
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

        ///// <summary>
        ///// 
        ///// </summary>
        //protected ExceptionHandler ExceptionHandler;

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
                Port = authorization.Port
            };
            Init();
        }

        protected void Init()
        {
            Connection = ConnectionFactory.CreateConnection();
            Channel = Connection.CreateModel();
            Connection.CallbackException += Connection_CallbackException;
            Channel.CallbackException += Channel_CallbackException;
        }

        //void ReConnection()
        //{
        //    Init(false);
        //}

        //protected void CheckChannel()
        //{
        //    if (Channel.IsClosed || !Connection.IsOpen)
        //    {
        //        ReConnection();
        //    }
        //    if (Channel.IsClosed || !Connection.IsOpen)
        //    {
        //        throw new Exception($"Channel or Connection is Closed! Time:{DateTime.Now}");
        //    }
        //}

        #region -- Event
        private void Channel_CallbackException(object sender, Events.CallbackExceptionEventArgs e)
        {
            Logger.Error($"Channel is CallbackException,Time:{DateTime.Now}", e.Exception);
            //ExceptionHandler?.Invoke("Channel Callback is exception", e.Exception);
        }

        private void Connection_CallbackException(object sender, Events.CallbackExceptionEventArgs e)
        {
            Logger.Error($"Connection is CallbackException,Time:{DateTime.Now}", e.Exception);
            //ExceptionHandler?.Invoke("Connection Callback is exception", e.Exception);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="arguments"></param>
        public void QueueDeclare(string queue, IDictionary<string, object> arguments = null)
        {
            //CheckChannel();
            Channel.QueueDeclare(queue, true, false, false, arguments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        public void QueueBind(string exchange, string queue, string routingKey)
        {
            //CheckChannel();
            Channel.QueueBind(queue, exchange, routingKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        /// <param name="arguments"></param>
        public void ExchangeDeclare(string exchange, ExchangeType exchangeType, IDictionary<string, object> arguments = null)
        {
            //CheckChannel();
            Channel.ExchangeDeclare(exchange, exchangeType.ToString().ToLower(), true, false, arguments);
        }
    }
}
