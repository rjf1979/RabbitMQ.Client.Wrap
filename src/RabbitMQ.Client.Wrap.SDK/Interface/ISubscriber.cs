using System;

namespace RabbitMQ.Client.Wrap.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISubscriber: IQueue, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string SubscribeHandler(string queue);
    }
}
