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
        string Subscribe(Func<string, bool> callBackEvent, uint prefetchSize = 10, ushort prefetchCount = 10);
    }
}
