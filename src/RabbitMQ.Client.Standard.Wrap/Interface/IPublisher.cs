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
        /// <param name="message"></param>
        void Publish(string message);

        void Publish<TObject>(TObject tObj);
    }
}
