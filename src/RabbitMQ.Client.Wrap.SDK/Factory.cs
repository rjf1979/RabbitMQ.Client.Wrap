using RabbitMQ.Client.Wrap.Config;
using RabbitMQ.Client.Wrap.Impl;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap
{
    /// <summary>
    /// 消息通道工厂
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// 创建一个订阅对象
        /// </summary>
        /// <param name="authorization"></param>
        /// <param name="subscribeEvent"></param>
        /// <returns></returns>
        public static ISubscriber CreateSubscriber(Authorization authorization, ISubscribeEvent subscribeEvent)
        {
            return new Subscriber(authorization, subscribeEvent);
        }


        /// <summary>
        /// 创建一个发布对象
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public static IPublisher CreatePublisher(Authorization authorization)
        {
            return new Publisher(authorization);
        }
    }
}
