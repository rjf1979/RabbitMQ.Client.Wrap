using System;
using RabbitMQ.Client.Wrap.Impl;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class Client
    {
        private readonly Authorization _authorization;

        private Client(string userName, string password, string vhost, string host, int port = 5672)
        {
            _authorization = new Authorization(userName, password, vhost, host, port);
        }
        private IPublisher _publisher;
        /// <summary>
        /// 生产者
        /// </summary>
        public IPublisher Publisher
        {
            get
            {
                lock (this)
                {
                    return _publisher ?? (_publisher = new Publisher(_authorization));
                }
            }
        }
        //private ISubscriber _subscriber;
        ///// <summary>
        ///// 消费者
        ///// </summary>
        //public ISubscriber Subscriber
        //{
        //    get
        //    {
        //        lock (this)
        //        {
        //            return _subscriber ?? (_subscriber = new Subscriber(_authorization));
        //        }
        //    }
        //}
        //public IPublisher CreatePublisher(string queueName)
        //{
        //    return new Publisher(_authorization, queueName);
        //}
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public ISubscriber CreateSubscriber(string queueName)
        {
            return new Subscriber(_authorization, queueName);
        }
        ///// <summary>
        ///// 注册一个异常处理
        ///// </summary>
        ///// <param name="exceptionHandler"></param>
        //public void RegisterExceptionHandler(Action<string, Exception> exceptionHandler)
        //{
        //    _publisher?.RegisterExceptionHandler(exceptionHandler);
        //    _subscriber?.RegisterExceptionHandler(exceptionHandler);
        //}

        /// <summary>
        /// 使用一个队列客户端
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="vhost"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Client Build(string userName, string password, string vhost, string host, int port = 5672)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(vhost)) throw new ArgumentNullException(nameof(vhost));
            if (string.IsNullOrEmpty(host)) throw new ArgumentNullException(nameof(host));
            if (port <= 0) throw new ArgumentException(nameof(port));
            return new Client(userName, password, vhost, host, port);
        }
    }
}
