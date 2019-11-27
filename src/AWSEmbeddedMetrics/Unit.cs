namespace AWSEmbeddedMetrics
{
    public sealed class Unit
    {
        private readonly string _metricUnitValue;

        private Unit(string metricUnitValue) => _metricUnitValue = metricUnitValue;

        public static readonly Unit Seconds = new Unit("Seconds");
        public static readonly Unit Microseconds = new Unit("Microseconds");
        public static readonly Unit Milliseconds = new Unit("Milliseconds");
        public static readonly Unit Bytes = new Unit("Bytes");
        public static readonly Unit Kilobytes = new Unit("Kilobytes");
        public static readonly Unit Megabytes = new Unit("Megabytes");
        public static readonly Unit Gigabytes = new Unit("Gigabytes");
        public static readonly Unit Terabytes = new Unit("Terabytes");
        public static readonly Unit Bits = new Unit("Bits");
        public static readonly Unit Kilobits = new Unit("Kilobits");
        public static readonly Unit Megabits = new Unit("Megabits");
        public static readonly Unit Gigabits = new Unit("Gigabits");
        public static readonly Unit Terabits = new Unit("Terabits");
        public static readonly Unit Percent = new Unit("Percent");
        public static readonly Unit Count = new Unit("Count");
        public static readonly Unit BytesPerSecond = new Unit("Bytes/Second");
        public static readonly Unit KilobytesPerSecond = new Unit("Kilobytes/Second");
        public static readonly Unit MegabytesPerSecond = new Unit("Megabytes/Second");
        public static readonly Unit GigabytesPerSecond = new Unit("Gigabytes/Second");
        public static readonly Unit TerabytesPerSecond = new Unit("Terabytes/Second");
        public static readonly Unit BitsPerSecond = new Unit("Bits/Second");
        public static readonly Unit KilobitsPerSecond = new Unit("Kilobits/Second");
        public static readonly Unit MegabitsPerSecond = new Unit("Megabits/Second");
        public static readonly Unit GigabitsPerSecond = new Unit("Gigabits/Second");
        public static readonly Unit TerabitsPerSecond = new Unit("Terabits/Second");
        public static readonly Unit CountPerSecond = new Unit("Count/Second");
        public static readonly Unit None = new Unit("None");

        public override string ToString()
            => _metricUnitValue;
    }
}
