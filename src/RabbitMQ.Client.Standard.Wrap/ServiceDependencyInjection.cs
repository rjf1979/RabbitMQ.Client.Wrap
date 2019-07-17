﻿using Microsoft.Extensions.DependencyInjection;
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
        public static void AddRabbitMqFactory(this IServiceCollection services)
        {
            services.AddSingleton<IFactory, Factory>();
        }
    }
}
