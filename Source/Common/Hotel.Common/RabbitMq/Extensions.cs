using System;
using Hotel.Common.RabbitMq.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Hotel.Common.RabbitMq
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,
            Action<RabbitMqOptions> setupAction)
        {
            services.Configure(setupAction);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMqModelPooledObjectPolicy>();

            services.AddSingleton<IRabbitMqManager, RabbitMqManager>();

            return services;
        }

        public static IServiceCollection AddRabbitMq<TConsumeBackgroundService>(this IServiceCollection services,
            Action<RabbitMqOptions> setupAction) where TConsumeBackgroundService : ConsumeRabbitMqBackgroundService
        {
            AddRabbitMq(services, setupAction);

            services.AddHostedService<TConsumeBackgroundService>();

            return services;
        }
    }
}