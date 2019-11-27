namespace AWSEmbeddedMetrics
{
    public interface IBuildableMetric
    {
        IBuildableMetric AddDimension(string key, string value);
        IBuildableMetric AddProperty(string key, string value);
        IBuildableMetric WithNamespace(string @namespace);
        Metric Build();
    }
}
