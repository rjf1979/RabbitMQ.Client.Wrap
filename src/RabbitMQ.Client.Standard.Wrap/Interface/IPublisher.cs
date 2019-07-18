using System;

namespace RabbitMQ.Client.Standard.Wrap.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublisher : IQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        void Publish(string queue,string message);
    }
}
