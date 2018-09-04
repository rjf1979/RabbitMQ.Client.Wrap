using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap
{
    public class RabbitMqConfigOption
    {
        /// <summary>
        /// The connection string. See https://www.rabbitmq.com/uri-spec.html for more information.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Durable (the queue will survive a broker restart)
        /// </summary>
        public bool IsQueueDurable { get; set; } = true;
        public string Topic { get; set; }
        public string Exchange { get; set; } = string.Empty;
        /// <summary>
        /// Durable (the exchange will survive a broker restart)
        /// </summary>
        public bool IsExchangeDurable { get; set; } = true;

        public ILogger Logger { get; set; } = new LoggerFactory().CreateLogger("RabbitMq");
        public ISerializer Serializer { get; set; } = new Serializer();
    }
}
