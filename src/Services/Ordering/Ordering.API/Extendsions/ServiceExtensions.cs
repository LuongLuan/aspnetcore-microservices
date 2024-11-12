using Infrastructure.Configurations;
using Infrastructure.Extendsions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ordering.API.Application.IntegrationEvents.EventsHanler;
using Shared.Configurations;
namespace Ordering.Infrastructure.Extendsions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        { 
            var emailSetting = configuration.GetSection(nameof(SMTPEmailSetting))
                .Get<SMTPEmailSetting>();
            services.AddSingleton(emailSetting);

            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
                .Get<DatabaseSettings>();
            services.AddSingleton(databaseSettings);

            return services;
        }
        
        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            services.AddHealthChecks()
                .AddSqlServer(databaseSettings.ConnectionString,
                    name: "SqlServer Health",
                    failureStatus: HealthStatus.Degraded);
        }
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSetting is not configured");

            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    // cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    // {
                    //     c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                    // });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
    
}
