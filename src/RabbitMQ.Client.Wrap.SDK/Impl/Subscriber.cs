using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Wrap.Config;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    internal class Subscriber : Queue, ISubscriber
    {
        readonly ISubscribeEvent _subscribeEvent;
        private readonly bool _isNeedNack;
        private readonly  Thread _checkThread;

        public Subscriber(Authorization authorization, ISubscribeEvent subscribeEvent, ExceptionHandler exceptionHandler = null, bool isNeedNack = true) : base(authorization, exceptionHandler)
        {
            _subscribeEvent = subscribeEvent;
            _isNeedNack = isNeedNack;
            _checkThread = new Thread(() =>
            {
                Thread.Sleep(60000);
                while (true)
                {
                    CheckChannel();
                    Thread.Sleep(5000);
                }
            });
            _checkThread.Start();
        }
        
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public string SubscribeHandler(string queue)
        {
            try
            {
                CheckChannel();
                var consumer = new EventingBasicConsumer(Channel);
                //接收事件
                consumer.Received += (ch, ea) =>
                {
                    var body = ea.Body;
                    var value = Encoding.UTF8.GetString(body);
                    if (_subscribeEvent != null)
                    {
                        var result = _subscribeEvent.Call(value);
                        if (result)
                        {
                            Channel.BasicAck(ea.DeliveryTag, false);
                        }
                        else
                        {
                            if (_isNeedNack) Channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    }
                };
                var consumerTag = Channel.BasicConsume(queue, false, consumer);
#if DEBUG
                Console.WriteLine(consumerTag);
#endif
                return consumerTag;
            }
            catch (Exception exp)
            {
                Logger.Error("Subscribe is exception", exp);
                ExceptionHandler?.Invoke("Subscribe is exception", exp);
            }
            return string.Empty;
        }

        public void Dispose()
        {
            _checkThread.Abort();
            Channel?.Dispose();
            Connection?.Dispose();
        }
    }
}
