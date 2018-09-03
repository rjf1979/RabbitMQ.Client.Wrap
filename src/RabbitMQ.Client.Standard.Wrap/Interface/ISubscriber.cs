using System;
using System.Threading.Tasks;

namespace RabbitMQ.Client.Standard.Wrap.Interface
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
        string Subscribe<TObject>(string queueName, Func<TObject, bool> callBackEvent);

        /// <summary>
        /// 设置消费预取设置
        /// </summary>
        /// <param name="prefetchSize"></param>
        /// <param name="prefetchCount"></param>
        /// <param name="global"></param>
        void BasicQos(uint prefetchSize = 0, ushort prefetchCount = 10,bool global=false);
    }
}
