using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Standard.Wrap.Interface;

namespace RabbitMQ.Client.Standard.Wrap.Impl
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class Factory: IFactory
    {
        private readonly ConcurrentDictionary<string,IPublisher> _publishers = new ConcurrentDictionary<string, IPublisher>();
        private readonly RabbitMQConfig _rabbitMqConfig;
        private readonly ILogger _logger;

        public Factory(ILogger logger,RabbitMQConfig rabbitMqConfig)
        {
            _logger = logger;
            _rabbitMqConfig = rabbitMqConfig;
        }

        /// <summary>
        /// 创建生产者
        /// </summary>
        public IPublisher GetPublisher(string name)
        {
            return _publishers.GetOrAdd(name, key =>
            {
                var option = _rabbitMqConfig.Options.FirstOrDefault(a=>a.Name == name);
                if (option == null)
                {
                    throw new ArgumentNullException($"not find config option:{name}");
                }
                return new Publisher(_logger,option);
            });
        }
    }
}
