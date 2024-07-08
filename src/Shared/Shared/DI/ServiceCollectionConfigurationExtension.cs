using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Shared.DI;
public static class ServiceCollectionConfigurationExtension
{
    public static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LoggingOption>(configuration.GetSection(LoggingOption.OptionName));
        services.Configure<AppConfigOption>(configuration.GetSection(AppConfigOption.OptionName));
        services.Configure<ElasticSearchOption>(configuration.GetSection(ElasticSearchOption.OptionName));
        services.Configure<IdentityGroupAccessOption>(configuration.GetSection(IdentityGroupAccessOption.OptionName));

        return services;
    }
}
