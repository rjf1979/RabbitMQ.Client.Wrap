using System;
using System.Text;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    internal class Publisher : Queue, IPublisher
    {
        private static readonly object _locker = new object();

        public void Publish(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            EnterLogEvent(LogLevel.Warn, "当前Channel：" + Channel.GetHashCode());
            lock (_locker)
            {
                var body = Encoding.UTF8.GetBytes(message);
                try
                {
                    if (Channel.IsOpen)
                    {
                        if (Option.ExchangeType == ExchangeType.Fanout)
                        {
                            Channel.BasicPublish(Option.Exchange, string.Empty, BasicProperties, body);
                        }
                        else if (Option.ExchangeType == ExchangeType.Direct)
                        {
                            Channel.BasicPublish(Option.Exchange, Option.RouteKey, BasicProperties, body);
                        }
                        else
                        {
                            Channel.BasicPublish(string.Empty, Option.Topic, BasicProperties, body);
                        }
                    }
                    else
                    {
                        var msg = "Channel is Not Opened";
                        EnterLogEvent(LogLevel.Error, msg);
                    }
                }
                catch (Exception exception)
                {
                    var msg = $"exchange：{Option.Exchange}，routingKey：{Option.Topic} > BasicPublish Failed,Time:{DateTime.Now}";
                    EnterLogEvent(LogLevel.Error, msg, exception, message);
                }
            }
        }

        public void Publish<TObject>(TObject tObj)
        {
            var message = Option.Serializer.Serialize(tObj);
            Publish(message);
        }

        //public void Dispose()
        //{
        //    Channel.Abort();
        //    Connection.Abort();
        //    Channel.Dispose();
        //    Connection.Dispose();
        //}

        public Publisher(RabbitMqConfigOption option) : base(option)
        {
            //如果Exchange不为空，需设定模式
            if (option.ExchangeType == ExchangeType.Fanout || option.ExchangeType == ExchangeType.Direct)
            {
                ExchangeDeclare(option.Exchange, option.ExchangeType);
            }
            else
            {
                QueueDeclare(option.Topic);
            }
        }
    }
}
