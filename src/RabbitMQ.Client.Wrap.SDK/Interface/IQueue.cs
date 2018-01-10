using System;
using System.Collections.Generic;

namespace RabbitMQ.Client.Wrap.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="arguments"></param>
        bool QueueDeclare(string queue, IDictionary<string, object> arguments = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        void QueueBind(string exchange, string queue, string routingKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        /// <param name="arguments"></param>
        void ExchangeDeclare(string exchange, ExchangeType exchangeType, IDictionary<string, object> arguments = null);
        /// <summary>
        /// 消息队列名称
        /// </summary>
        string QueueName { get; }
        /// <summary>
        /// 注册记录异常日志
        /// </summary>
        /// <param name="exceptionAction"></param>
        void RegisterExceptionHandler(Action<string, Exception> exceptionAction);
    }
}
