using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Wrap.Interface;

namespace RabbitMQ.Client.Wrap.Impl
{
    internal class Publisher : Queue, IPublisher
    {
        internal Publisher(Authorization authorization) : base(authorization)
        {

        }

        public async Task Publish(string routingKey, string message)
        {
            await Publish(string.Empty, routingKey, message);
        }

        public async Task Publish(string exchange, string routingKey, string message)
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
            await Task.Run(() =>
            {
                lock (this)
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    try
                    {
                        Channel.BasicPublish(exchange, routingKey, BasicProperties, body);
#if DEBUG
                        //Logger.Console($"Enter Queue：{routingKey}，Message：{message} > {DateTime.Now}");
#endif
                    }
                    catch (Exception exception)
                    {
                        Logger.Error($"Channel BasicPublish Failed,Time:{DateTime.Now}", exception);
                    }
                }
            });
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
