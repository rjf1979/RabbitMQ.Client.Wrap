using System;

namespace RabbitMQ.Client.Standard.Wrap.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublisher :IQueue ,IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Publish(string message);

        void Publish<TObject>(TObject tObj);
    }
}
