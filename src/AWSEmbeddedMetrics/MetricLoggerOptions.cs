using System;
using Microsoft.Extensions.DependencyInjection;

namespace AWSEmbeddedMetrics
{
    public sealed class MetricLoggerOptions
    {
        public static readonly MetricLoggerOptions Default = new MetricLoggerOptions();
        public string DefaultNamespace { get; set; } = "aws-embedded-metrics";
    }

    public static class MetricLoggerExtensions
    {
        public static IServiceCollection AddMetricLogger(this IServiceCollection services)
        {
            return AddMetricLogger(services, MetricLoggerOptions.Default);
        }

        public static IServiceCollection AddMetricLogger(this IServiceCollection services, MetricLoggerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return services.AddSingleton<IMetricLogger>(new MetricLogger(options));
        }

        public static IServiceCollection AddMetricLogger(this IServiceCollection services,
            Action<MetricLoggerOptions> configureOptions)
        {
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            var options = new MetricLoggerOptions();
            configureOptions.Invoke(options);
            return services.AddMetricLogger(options);
        }
    }
}
