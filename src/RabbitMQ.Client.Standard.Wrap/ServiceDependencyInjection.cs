using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Standard.Wrap.Impl;

namespace RabbitMQ.Client.Standard.Wrap
{
    public static class ServiceDependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection"></param>
        public static void AddRabbitMqFactory(this IServiceCollection services,IConfigurationSection configurationSection)
        {
            services.Configure<RabbitMQConfig>(configurationSection);
            services.AddSingleton<IRabbitMQFactory, RabbitMQFactory>();
        }
    }
}
