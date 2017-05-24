using System;
using System.Threading.Tasks;

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
        string Subscribe(string queue, Func<string, bool> callBackEvent);

        //void RegisterSubscribe(string queue, Func<string, bool> callBack);
    }
}
