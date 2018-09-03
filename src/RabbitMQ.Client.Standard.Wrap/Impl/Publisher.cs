using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    internal class Publisher : Queue, IPublisher
    {
        public Task PublishAsync(string topic, string message)
        {
            return PublishAsync(string.Empty, topic, message);
        }

        public Task PublishAsync<TObject>(string topic, TObject tObj)
        {
            var message = Option.Serializer.Serialize(tObj);
            return PublishAsync(topic, message);
        }

        public Task PublishAsync(string exchange, string topic, string message)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }
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
                        Channel.BasicPublish(exchange, topic, BasicProperties, body);
                    }
                    else
                    {
                        var msg = "Channel is Not Opened";
                        EnterLogEvent(LogLevel.Error, msg);
                    }
                }
                catch (Exception exception)
                {
                    var msg = $"exchange：{exchange}，routingKey：{topic} > BasicPublish Failed,Time:{DateTime.Now}";
                    EnterLogEvent(LogLevel.Error, msg, exception, message);
                }
            }

            return Task.CompletedTask;
        }

        public Task PublishAsync<TObject>(string exchange, string topic, TObject tObj)
        {
            var message = Option.Serializer.Serialize(tObj);
            return PublishAsync(exchange, topic, message);
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
