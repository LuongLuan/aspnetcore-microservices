using Infrastructure.Configurations;
using Infrastructure.Extendsions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
    }
    
}
