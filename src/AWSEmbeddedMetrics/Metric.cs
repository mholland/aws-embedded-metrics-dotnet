using System.Collections.Generic;

namespace AWSEmbeddedMetrics
{
    public sealed class Metric
    {
        public string Name { get; }
        public double Value { get; }
        public string Unit { get; }
        public IReadOnlyDictionary<string, string> Dimensions { get; }
        public IReadOnlyDictionary<string, string> Properties { get; }
        public string Namespace { get; }

        internal Metric(string name, double value, string unit, IReadOnlyDictionary<string, string> dimensions,
            IReadOnlyDictionary<string, string> properties, string @namespace)
        {
            Name = name;
            Value = value;
            Unit = unit;
            Dimensions = dimensions;
            Properties = properties;
            Namespace = @namespace;
        }
    }
}
