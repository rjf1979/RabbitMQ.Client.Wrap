using System;
using RabbitMQ.Client.Standard.Wrap.Impl;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class MessageBusClient
    {
        private readonly RabbitMqConfigOption _option;
        private MessageBusClient(RabbitMqConfigOption option)
        {
            _option = option;
        }

        /// <summary>
        /// 创建生产者
        /// </summary>
        public IPublisher CreatePublisher()
        {
            return new Publisher(_option);
        }
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public ISubscriber CreateSubscriber(string queueName)
        {
            return new Subscriber(_option);
        }

        /// <summary>
        /// 使用一个队列客户端
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static MessageBusClient Build(RabbitMqConfigOption option)
        {
            if (string.IsNullOrEmpty(option.ConnectionString)) throw new ArgumentNullException(nameof(option.ConnectionString));
            return new MessageBusClient(option);
        }
    }
}
