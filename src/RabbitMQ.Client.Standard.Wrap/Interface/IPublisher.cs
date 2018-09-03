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
        /// <param name="message"></param>
        Task PublishAsync(string message);
        Task PublishAsync<TObject>(TObject tObj);
    }
}
