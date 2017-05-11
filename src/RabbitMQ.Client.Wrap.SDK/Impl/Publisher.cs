using System;
using System.Text;
using RabbitMQ.Client.Content;
using RabbitMQ.Client.Wrap.Config;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    internal class Publisher : Queue, IPublisher
    {
        internal Publisher(Authorization authorization, ExceptionHandler exceptionHandler = null) : base(authorization, exceptionHandler)
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
            lock (this)
            {
                CheckChannel();
                var body = Encoding.UTF8.GetBytes(message);
                if (BasicProperties == null)
                {
                    var mapMessageBuilder = new MapMessageBuilder(Channel);
                    BasicProperties = (IBasicProperties)mapMessageBuilder.GetContentHeader();
                }
                try
                {
                    Channel.BasicPublish(exchange, routingKey, BasicProperties, body);
                }
                catch (Exception exception)
                {
                    Logger.Error($"Channel BasicPublish Failed,Time:{DateTime.Now}", exception);
                    ExceptionHandler?.Invoke("Publish is exception", exception);
                }
            }
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
