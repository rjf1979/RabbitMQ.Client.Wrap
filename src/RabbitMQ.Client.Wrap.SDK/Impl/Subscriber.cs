using System;
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
                if (Channel.IsClosed)
                {
                    var message = "queue:{queue} > Channel is not opened";
                    EnterLogEvent(LogLevel.Error, message);
                    throw new Exception(message);
                }
                var consumer = new EventingBasicConsumer(Channel);
                //接收事件
                consumer.Received += async (sender, ea) =>
                {
                    var localConsumer = (EventingBasicConsumer)sender;
                    await Task.Run(() =>
                    {
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
                    });
                };
                if (Channel != null)
                {
                    var consumerTag = Channel.BasicConsume(queue, _noAck, consumer);
                    return consumerTag;
                }
                var msg = $"Channel is null，Queue：{queue}";
                var exp = new Exception(msg);
                EnterLogEvent(LogLevel.Error, msg);
                throw exp;
            }
            catch (Exception exp)
            {
                var msg = $"Subscribe is exception，Queue：{queue}";
                EnterLogEvent(LogLevel.Error, msg, exp);
                throw;
            }
        }

        public void Dispose()
        {
            Channel?.Dispose();
            Connection?.Dispose();
        }
    }
}
