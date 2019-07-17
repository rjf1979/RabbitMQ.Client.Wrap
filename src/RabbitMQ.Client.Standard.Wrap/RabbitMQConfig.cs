using System.Collections.Generic;

namespace RabbitMQ.Client.Standard.Wrap
{
    /// <summary>
    /// 配置
    /// </summary>
    public class RabbitMQConfig
    {
        public IList<Option> Options { get; set; }

        /// <summary>
        /// 配置选项
        /// </summary>
        public class Option
        {
            /// <summary>
            /// 队列唯一名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// The connection string. See https://www.rabbitmq.com/uri-spec.html for more information.
            /// </summary>
            public string ConnectionString { get; set; }
        }
    }
}
