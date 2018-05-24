using System;
using System.Text;
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
        /// <param name="queueName"></param>
        /// <param name="noAck">是否无需等待回答就默认消费掉消息</param>
        /// <param name="isNeedNack">消息是否需要重新丢入队列</param>
        public Subscriber(Authorization authorization, string queueName, bool noAck = false, bool isNeedNack = true) : base(authorization, queueName)
        {
            _isNeedNack = isNeedNack;
            _noAck = noAck;

        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="callBackEvent"></param>
        /// <returns></returns>
        public string Subscribe(Func<string, bool> callBackEvent)
        {
            try
            {
                if (Channel.IsClosed)
                {
                    var message = "queue:{queue} > Channel is not opened";
                    EnterLogEvent(LogLevel.Error, message);
                    throw new Exception(message);
                }
                //Channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(Channel);
                //接收事件
                consumer.Received += (sender, ea) =>
                {
                    var localConsumer = (EventingBasicConsumer)sender;
                    var body = ea.Body;
                    var value = Encoding.UTF8.GetString(body);
                    lock (this)
                    {
                        var result = callBackEvent?.Invoke(value);
                        if (result != null && result.Value)
                        {
                            if (localConsumer.Model.IsOpen)
                                localConsumer.Model.BasicAck(ea.DeliveryTag, false);
                            else
                            {
                                var message = "queue:{queue} > BasicAck，Channel is not opened";
                                EnterLogEvent(LogLevel.Error, message);
                            }
                        }
                        else
                        {
                            if (_isNeedNack)
                            {
                                if (localConsumer.Model.IsOpen)
                                    localConsumer.Model.BasicNack(ea.DeliveryTag, false, true);
                                else
                                {
                                    var message = "queue:{queue} > BasicNack，Channel is not opened";
                                    EnterLogEvent(LogLevel.Error, message);
                                }
                            }
                        }
                    }
                };
                if (Channel != null)
                {
                    var consumerTag = Channel.BasicConsume(QueueName, _noAck, consumer);
                    return consumerTag;
                }
                var msg = $"Channel is null，Queue：{QueueName}";
                var exp = new Exception(msg);
                EnterLogEvent(LogLevel.Error, msg);
                throw exp;
            }
            catch (Exception exp)
            {
                var msg = $"Subscribe is exception，Queue：{QueueName}";
                EnterLogEvent(LogLevel.Error, msg, exp);
                throw;
            }
        }

        public void BasicQos(uint prefetchSize = 0, ushort prefetchCount = 10, bool global = false)
        {
            Channel.BasicQos(prefetchSize, prefetchCount, global);
        }

        public void Dispose()
        {
            Channel?.Dispose();
            Connection?.Dispose();
        }
    }
}
