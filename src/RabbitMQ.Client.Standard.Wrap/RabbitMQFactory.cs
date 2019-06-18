using RabbitMQ.Client.Standard.Wrap.Impl;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class RabbitMqFactory
    {
        /// <summary>
        /// 创建生产者
        /// </summary>
        public static IPublisher CreatePublisher(RabbitMqConfigOption option)
        {
            return new Publisher(option);
        }
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static ISubscriber CreateSubscriber(RabbitMqConfigOption option)
        {
            return new Subscriber(option);
        }
    }
}
