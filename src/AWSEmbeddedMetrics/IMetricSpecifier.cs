namespace AWSEmbeddedMetrics
{
    public interface IMetricSpecifier
    {
        IBuildableMetric SpecifyMetric(string metricName, double metricValue, Unit metricUnit = null);
    }
}
