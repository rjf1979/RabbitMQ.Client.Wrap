using System;
using System.Threading.Tasks;

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
        /// <param name="topic"></param>
        /// <param name="message"></param>
        Task PublishAsync(string topic, string message);
        Task PublishAsync<TObject>(string topic, TObject tObj);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        Task PublishAsync(string exchange, string topic, string message);
        Task PublishAsync<TObject>(string exchange, string topic, TObject tObj);
    }
}
