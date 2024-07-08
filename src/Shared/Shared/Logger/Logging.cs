using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Shared.Options;

namespace Shared.Logger;
public class Logging
{
    public static ILogger GetLogger(IConfiguration configuration, IWebHostEnvironment env, string logIndexPattern)
    {
        var loggingOptions = configuration.GetSection(LoggingOption.OptionName).Get<LoggingOption>();
        var appConfigurations = configuration.GetSection(AppConfigOption.OptionName).Get<AppConfigOption>();
        var elasticUri = configuration.GetSection(ElasticSearchOption.OptionName).Get<ElasticSearchOption>();

        Enum.TryParse(loggingOptions.Console.LogLevel, false, out LogEventLevel minimumEventLevel);

        var loggerConfiguartion = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch(minimumEventLevel))
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
            .Enrich.WithProperty(nameof(appConfigurations.ApplicationIdentifier), appConfigurations.ApplicationIdentifier)
            .Enrich.WithProperty(nameof(appConfigurations.ApplicationEnvironment), appConfigurations.ApplicationEnvironment);

        if (loggingOptions.Console.Enabled)
        {
            loggerConfiguartion.WriteTo.Console(minimumEventLevel, loggingOptions.LogOutputTemplate);
        }
        if (loggingOptions.Elastic.Enabled)
        {
            loggerConfiguartion.WriteTo.Elasticsearch(elasticUri.Uri, logIndexPattern);
        }

        return loggerConfiguartion
            .Destructure
            .UsingAttributes()
            .CreateLogger();
    }
}
