using System;

namespace AWSEmbeddedMetrics.Tests
{
    internal sealed class TestClock : ISystemClock
    {
        public TestClock()
            : this(DateTimeOffset.UtcNow)
        {
        }

        public TestClock(DateTimeOffset testTime)
            => UtcNow = testTime;

        public DateTimeOffset UtcNow { get; }
    }
}
