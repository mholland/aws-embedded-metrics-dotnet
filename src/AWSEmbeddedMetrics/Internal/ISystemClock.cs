using System;

namespace AWSEmbeddedMetrics.Internal
{
    internal interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
