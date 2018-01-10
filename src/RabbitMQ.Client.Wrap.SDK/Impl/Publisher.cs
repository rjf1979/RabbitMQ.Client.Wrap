using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    internal class Publisher : Queue, IPublisher
    {
        internal Publisher(Authorization authorization) : base(authorization, string.Empty)
        {

        }

        public void Publish(string routingKey, string message)
        {
            Publish(string.Empty, routingKey, message);
        }

        public void Publish(string exchange, string routingKey, string message)
        {
            if (string.IsNullOrWhiteSpace(routingKey))
            {
                throw new ArgumentNullException(nameof(routingKey));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            //if (BasicProperties == null)
            //{
            //    var mapMessageBuilder = new MapMessageBuilder(Channel);
            //    BasicProperties = (IBasicProperties)mapMessageBuilder.GetContentHeader();
            //}

            Task.Run(() =>
            {
                lock (this)
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    try
                    {
                        if (Channel.IsOpen)
                        {
                            Channel.BasicPublish(exchange, routingKey, BasicProperties, body);
                        }
                        else
                        {
                            var msg = "Channel is Not Opened";
                            EnterLogEvent(LogLevel.Error, msg);
                        }
                    }
                    catch (Exception exception)
                    {
                        var msg = $"exchange：{exchange}，routingKey：{routingKey} > BasicPublish Failed,Time:{DateTime.Now}";
                        EnterLogEvent(LogLevel.Error, msg, exception, message);
                    }
                }
            }).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Channel.Abort();
            Connection.Abort();
            Channel.Dispose();
            Connection.Dispose();
        }
    }
}
