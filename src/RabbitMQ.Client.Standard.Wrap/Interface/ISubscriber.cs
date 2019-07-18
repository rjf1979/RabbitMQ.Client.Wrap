﻿using System;

namespace RabbitMQ.Client.Standard.Wrap.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISubscriber: IQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Subscribe(string queue,Action<string> callBackEvent);

        /// <summary>
        /// 设置消费预取设置
        /// </summary>
        /// <param name="prefetchSize"></param>
        /// <param name="prefetchCount"></param>
        /// <param name="global"></param>
        void BasicQos(uint prefetchSize = 0, ushort prefetchCount = 10,bool global=false);
    }
}
