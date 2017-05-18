using System;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Wrap.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublisher :IQueue ,IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        Task Publish(string routingKey, string message);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        Task Publish(string exchange, string routingKey, string message);
    }
}
