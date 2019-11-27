namespace AWSEmbeddedMetrics
{
    public interface IMetricLogger
    {
        void Log(Metric metric);
    }
}
