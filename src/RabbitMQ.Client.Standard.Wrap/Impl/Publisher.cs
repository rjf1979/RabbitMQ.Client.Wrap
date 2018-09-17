using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    internal class Publisher : Queue, IPublisher
    {
        public Task PublishAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            lock (this)
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

            return Task.CompletedTask;
        }

        public Task PublishAsync<TObject>(TObject tObj)
        {
            var message = Option.Serializer.Serialize(tObj);
            return PublishAsync(message);
        }

        public void Dispose()
        {
            Channel.Abort();
            Connection.Abort();
            Channel.Dispose();
            Connection.Dispose();
        }

        public Publisher(RabbitMqConfigOption option) : base(option)
        {
        }
    }
}
