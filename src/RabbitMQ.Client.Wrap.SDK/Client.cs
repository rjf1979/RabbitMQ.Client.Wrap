using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Wrap.Impl;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap
{
    public class Client
    {
        //private static readonly Client _client;
        private readonly Authorization _authorization;

        private Client(string userName, string password, string vhost, string host, int port = 5672)
        {
            _authorization = new Authorization(userName, password, vhost, host, port);
        }

        private IPublisher _publisher;
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

        private ISubscriber _subscriber;

        public ISubscriber Subscriber
        {
            get
            {
                lock (this)
                {
                    return _subscriber ?? (_subscriber = new Subscriber(_authorization));
                }
            }
        }

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
