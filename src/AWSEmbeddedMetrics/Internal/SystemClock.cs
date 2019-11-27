using System;

namespace AWSEmbeddedMetrics.Internal
{
    internal sealed class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}