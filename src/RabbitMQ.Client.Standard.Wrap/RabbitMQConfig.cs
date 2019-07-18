using System.Collections.Generic;

namespace RabbitMQ.Client.Standard.Wrap
{
    /// <summary>
    /// 配置
    /// </summary>
    public class RabbitMQConfig
    {

        public IList<RabbitMQConfigOption> Options { get; set; }
    }

    public class RabbitMQConfigOption
    {
        /// <summary>
        /// 队列唯一名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The connection string. See https://www.rabbitmq.com/uri-spec.html for more information.
        /// </summary>
        public string Connection { get; set; }
    }
}
