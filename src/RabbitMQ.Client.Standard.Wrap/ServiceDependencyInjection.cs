using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Standard.Wrap.Impl;

namespace RabbitMQ.Client.Standard.Wrap
{
    public static class ServiceDependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public static void AddRabbitMqFactory(this IServiceCollection services, string name, IOptions<RabbitMQConfig> config)
        {
            services.AddSingleton<IFactory>(opt => new Factory(opt.GetService<ILogger>(), config.Value));
        }
    }
}
