﻿using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Extendsions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.EventLog;
using Shared.Configurations;

namespace Basket.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings (this IServiceCollection services,
            IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);
            var cacheSettings = configuration.GetSection(nameof(CacheSettings))
                .Get<CacheSettings>();
            services.AddSingleton(cacheSettings);

            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)=>
             services.AddScoped<IBasketRepository, BasketRepository>()
            .AddTransient<ISerializeService, SerializeService>()
        ;
        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = services.GetOptions<CacheSettings>("CacheSettings");
            if (string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("Redis Connection string is not configured.");

            //Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = settings.ConnectionString;
            });
        }
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress) ||
                string.IsNullOrEmpty(settings.HostAddress)) throw new ArgumentNullException("EventBusSettings is not configured!");

            var mqConnection = new Uri(settings.HostAddress);

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                // Publish submit order message, instead of sending it to a specific queue directly.
                config.AddRequestClient<IBasketCheckoutEvent>();
            });
        }
    }
}
