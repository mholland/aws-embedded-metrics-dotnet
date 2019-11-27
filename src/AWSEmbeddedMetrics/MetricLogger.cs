using System;
using AWSEmbeddedMetrics.Internal;

namespace AWSEmbeddedMetrics
{
    public sealed class MetricLogger : IMetricLogger
    {
        private readonly Serialiser _serialiser;

        public MetricLogger(MetricLoggerOptions options)
            => _serialiser = new Serialiser(new SystemClock(), options);

        public void Log(Metric metric)
            => Console.WriteLine(_serialiser.SerialiseMetric(metric));
    }
}
