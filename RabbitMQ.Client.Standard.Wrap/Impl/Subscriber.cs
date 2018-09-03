using System;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    internal class Subscriber : Queue, ISubscriber
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="callBackEvent"></param>
        /// <returns></returns>
        public string Subscribe<TObject>(string queueName, Func<TObject, bool> callBackEvent)
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
                consumer.Received += (sender, ea) =>
                {
                    var localConsumer = (EventingBasicConsumer)sender;
                    var body = ea.Body;
                    var value = Encoding.UTF8.GetString(body);
                    lock (this)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(value)) return;
                            var tobj = Option.Serializer.Deserialize<TObject>(value);
                            var result = callBackEvent.Invoke(tobj);
                            if (result)
                            {
                                if (localConsumer.Model.IsOpen)
                                    localConsumer.Model.BasicAck(ea.DeliveryTag, false);
                                else
                                {
                                    var message = "queue:{queue} > BasicAck，Channel is not opened";
                                    EnterLogEvent(LogLevel.Error, message);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            var message = "queue:{queue} > BasicAck，callBackEvent.Invoke is exception";
                            EnterLogEvent(LogLevel.Error, message, exception, value);
                        }
                    }
                };
                if (Channel != null)
                {
                    var consumerTag = Channel.BasicConsume(queueName, false, consumer);
                    return consumerTag;
                }
                var msg = $"Channel is null，Queue：{queueName}";
                var exp = new Exception(msg);
                EnterLogEvent(LogLevel.Error, msg);
                throw exp;
            }
            catch (Exception exp)
            {
                var msg = $"Subscribe is exception，Queue：{queueName}";
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

        public Subscriber(RabbitMqConfigOption option) : base(option)
        {
        }
    }
}
