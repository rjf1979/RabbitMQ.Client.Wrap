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
        //readonly ISubscribeEvent _subscribeEvent;
        readonly IDictionary<string, Func<string, bool>> _callBackEvents = new Dictionary<string, Func<string, bool>>();
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
        /// <returns></returns>
        public string Subscribe(string queue)
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
                            Func<string, bool> callBackEvent;
                            if (_callBackEvents.TryGetValue(queue, out callBackEvent))
                            {
                                var result = callBackEvent(value);
                                if (result)
                                {
                                    Channel.BasicAck(ea.DeliveryTag, false);
                                }
                                else
                                {
                                    if (_isNeedNack) Channel.BasicNack(ea.DeliveryTag, false, true);
                                }
                            }
                            else
                            {
                                var msg = "Subscribe callback is not exist!";
                                Logger.Error(msg);
                                throw new Exception(msg);
                            }
                        }
                    });
                };
                var consumerTag = Channel.BasicConsume(queue, _noAck, consumer);
#if DEBUG
                Console.WriteLine($"consumer > " + consumerTag);
#endif
                return consumerTag;
            }
            catch (Exception exp)
            {
                var msg = $"Subscribe is exception，Queue：{queue}";
                Logger.Error(msg, exp);
                throw;
            }
        }

        public void RegisterSubscribe(string queue, Func<string, bool> callBack)
        {
            lock (this)
            {
                if (!_callBackEvents.ContainsKey(queue)) _callBackEvents.Add(queue, callBack);
            }
        }

        public void Dispose()
        {
            Channel?.Dispose();
            Connection?.Dispose();
        }
    }
}
