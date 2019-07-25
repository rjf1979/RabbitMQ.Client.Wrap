using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Standard.Wrap.Interface;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class RabbitMQFactory: IRabbitMQFactory
    {
        private readonly ConcurrentDictionary<string,IPublisher> _publishers = new ConcurrentDictionary<string, IPublisher>();
        private readonly RabbitMQConfig _config;
        private readonly ILogger _logger;

        public RabbitMQFactory(ILogger logger,IOptions<RabbitMQConfig> options)
        {
            _logger = logger;
            _config = options.Value;
        }

        public RabbitMQFactory(ILogger logger, RabbitMQConfig config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// 创建生产者
        /// </summary>
        public IPublisher GetPublisher(string name)
        {
            return _publishers.GetOrAdd(name, key =>
            {
                var option = _config.Options.FirstOrDefault(a=>a.Name == name);
                if (option == null)
                {
                    throw new ArgumentNullException($"not find config option:{name}");
                }
                return new Publisher(_logger,option);
            });
        }

        public ISubscriber GetSubscriber(string name)
        {
            var option = _config.Options.FirstOrDefault(a => a.Name == name);
            if (option == null)
            {
                throw new ArgumentNullException($"not find config option:{name}");
            }

            return new Subscriber(_logger, option);
        }
    }
}
