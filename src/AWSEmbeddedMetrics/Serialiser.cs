using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using AWSEmbeddedMetrics.Internal;

namespace AWSEmbeddedMetrics
{
    internal sealed class Serialiser
    {
        private readonly MetricLoggerOptions _options;
        private readonly ISystemClock _systemClock;

        public Serialiser(ISystemClock systemClock, MetricLoggerOptions options)
        {
            _options = options;
            _systemClock = systemClock ?? new SystemClock();
        }
        public string SerialiseMetric(Metric metric)
        {
            using var ms = new MemoryStream();
            using var writer = new Utf8JsonWriter(ms);

            // CloudWatch supports a maximum of 10 dimensions.
            var dimensions = metric.Dimensions.Take(10).ToDictionary(x => x.Key, x => x.Value);

            writer.WriteStartObject();
            writer.WriteStartObject("_aws");

            writer.WriteNumber("Timestamp", _systemClock.UtcNow.ToUnixTimeSeconds());

            writer.WriteStartArray("CloudWatchMetrics");
            writer.WriteStartObject();

            writer.WriteString("Namespace", metric.Namespace ?? _options.DefaultNamespace);

            writer.WriteStartArray("Dimensions");
            WriteDimensionReferences(writer, dimensions);
            writer.WriteEndArray();

            writer.WriteStartArray("Metrics");
            WriteMetricDefinition(writer, metric.Name, metric.Unit);
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.WriteEndArray();

            writer.WriteEndObject();

            WriteTargetMembers(writer, dimensions);
            WriteTargetMembers(writer, metric.Properties);
            writer.WriteNumber(metric.Name, metric.Value);

            writer.WriteEndObject();

            writer.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static void WriteMetricDefinition(Utf8JsonWriter writer, string name, string unit)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", name);
            writer.WriteString("Unit", unit);
            writer.WriteEndObject();
        }

        private static void WriteDimensionReferences(Utf8JsonWriter writer, IReadOnlyDictionary<string, string> dimensions)
        {
            writer.WriteStartArray();
            foreach (var key in dimensions.Keys)
            {
                writer.WriteStringValue(key);
            }
            writer.WriteEndArray();
        }

        private static void WriteTargetMembers(Utf8JsonWriter writer, IReadOnlyDictionary<string, string> pairs)
        {
            foreach (var property in pairs)
            {
                writer.WriteString(property.Key, property.Value);
            }
        }
    }
}
