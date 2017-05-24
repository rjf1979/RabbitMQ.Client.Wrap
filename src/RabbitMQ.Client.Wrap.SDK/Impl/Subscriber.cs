using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    internal class Subscriber : Queue, ISubscriber
    {
        private readonly bool _isNeedNack;
        private readonly bool _noAck;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        /// <param name="noAck">是否无需等待回答就默认消费掉消息</param>
        /// <param name="isNeedNack">消息是否需要重新丢入队列</param>
        public Subscriber(Authorization authorization, bool noAck = false, bool isNeedNack = true) : base(authorization)
        {
            _isNeedNack = isNeedNack;
            _noAck = noAck;
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="callBackEvent"></param>
        /// <returns></returns>
        public string Subscribe(string queue, Func<string, bool> callBackEvent)
        {
            try
            {
                var consumer = new EventingBasicConsumer(Channel);
                //接收事件
                consumer.Received += async (ch, ea) =>
                {
                    await Task.Run(() =>
                    {
                        var body = ea.Body;
                        var value = Encoding.UTF8.GetString(body);
                        lock (this)
                        {
                            var result = callBackEvent?.Invoke(value);
                            if (result != null && result.Value)
                            {
                                if(Channel.IsOpen) Channel.BasicAck(ea.DeliveryTag, false);
                            }
                            else
                            {
                                if (_isNeedNack)
                                {
                                    if (Channel.IsOpen)
                                        Channel.BasicNack(ea.DeliveryTag, false, true);
                                }
                            }
                        }
                    });
                };
                if (Channel != null)
                {
                    var consumerTag = Channel.BasicConsume(queue, _noAck, consumer);
#if DEBUG
                    Console.WriteLine($"consumer > " + consumerTag);
#endif
                    return consumerTag;
                }
                var msg = $"Channel is null，Queue：{queue}";
                var exp = new Exception(msg);
                Logger.Error(msg, exp);
                ExceptionHandler?.Invoke(msg, exp);
                throw exp;
            }
            catch (Exception exp)
            {
                var msg = $"Subscribe is exception，Queue：{queue}";
                Logger.Error(msg, exp);
                ExceptionHandler?.Invoke(msg, exp);
                throw;
            }
        }

        //public void RegisterSubscribe(string queue, Func<string, bool> callBack)
        //{
        //    lock (this)
        //    {
        //        if (!_callBackEvents.ContainsKey(queue)) _callBackEvents.Add(queue, callBack);
        //    }
        //}

        public void Dispose()
        {
            Channel?.Dispose();
            Connection?.Dispose();
        }
    }
}
