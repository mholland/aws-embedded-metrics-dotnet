using System;
using System.Collections.Generic;

namespace AWSEmbeddedMetrics
{
    public sealed class MetricBuilder : IMetricSpecifier, IBuildableMetric
    {
        private string _metricName;
        private double _metricValue;
        private Unit _metricUnit;
        private readonly Dictionary<string, string> _dimensions = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private string _namespace;

        public static IMetricSpecifier Create() => new MetricBuilder();

        private MetricBuilder()
        {
        }

        public IBuildableMetric SpecifyMetric(string metricName, double metricValue, Unit metricUnit = null)
        {
            _metricValue = metricValue;
            _metricName = metricName;
            _metricUnit = metricUnit ?? Unit.None;
            return this;
        }

        public IBuildableMetric AddDimension(string key, string value)
        {
            _dimensions.Add(key, value);
            return this;
        }

        public IBuildableMetric AddProperty(string key, string value)
        {
            _properties.Add(key, value);
            return this;
        }

        public IBuildableMetric WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public Metric Build()
        {
            if (string.IsNullOrEmpty(_metricName))
                throw new ArgumentException("MetricName must be provided");

            return new Metric(_metricName, _metricValue, _metricUnit.ToString(), _dimensions, _properties, _namespace);
        }
    }
}
