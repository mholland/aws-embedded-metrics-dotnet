using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AWSEmbeddedMetrics.Tests
{
    public class MetricBuilderTests
    {
        [Fact]
        public void ThenTheMetricNameIsSpecified()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Latency", 0.0)
                .Build()
                .Name
                .Should()
                .Be("Latency");

        [Fact]
        public void ThenTheMetricValueIsSpecified()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Latency", 42.0)
                .Build()
                .Value
                .Should()
                .Be(42.0);

        [Fact]
        public void ThenTheMetricUnitIsSpecified()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Latency", 42.0, Unit.Gigabits)
                .Build()
                .Unit
                .Should()
                .Be("Gigabits");

        [Fact]
        public void WhenNoUnitIsSpecifiedNoneIsUsed()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Latency", 42.0)
                .Build()
                .Unit
                .Should()
                .Be("None");

        [Fact]
        public void ThenDimensionsAreSpecified()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Foo", 42)
                .AddDimension("TowelId", "abc")
                .Build()
                .Dimensions
                .Should()
                .Contain(new KeyValuePair<string, string>("TowelId", "abc"));

        [Fact]
        public void ThenPropertiesAreSpecified()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Foo", 42)
                .AddProperty("Zaphod", "Beeblebrox")
                .Build()
                .Properties
                .Should()
                .Contain(new KeyValuePair<string, string>("Zaphod", "Beeblebrox"));

        [Fact]
        public void ThenNamespaceIsSpecified()
            => MetricBuilder
                .Create()
                .SpecifyMetric("Latency", 42, Unit.Seconds)
                .WithNamespace("Heart-Of-Gold")
                .Build()
                .Namespace
                .Should()
                .Be("Heart-Of-Gold");

        [Fact]
        public void WhenInvalidMetricNameIsProvidedThenTheMetricCannotBeBuilt()
            => MetricBuilder
                .Create()
                .SpecifyMetric(string.Empty, 0.0)
                .Invoking(bm => bm.Build())
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("*MetricName*");

        [Theory]
        [MemberData(nameof(UnitTestData))]
        public void WhenAUnitIsSpecifiedThenItIsAValidCloudWatchUnitValue(Unit unit, string expectedUnit)
            => MetricBuilder
                .Create()
                .SpecifyMetric("Arthur", 42, unit)
                .Build()
                .Unit
                .Should()
                .Be(expectedUnit);

        public static IEnumerable<object[]> UnitTestData
            => new List<object[]>
            {
                new object[] {Unit.Seconds, "Seconds"},
                new object[] {Unit.Microseconds, "Microseconds"},
                new object[] {Unit.Milliseconds, "Milliseconds"},
                new object[] {Unit.Bytes, "Bytes"},
                new object[] {Unit.Kilobytes, "Kilobytes"},
                new object[] {Unit.Megabytes, "Megabytes"},
                new object[] {Unit.Gigabytes, "Gigabytes"},
                new object[] {Unit.Terabytes, "Terabytes"},
                new object[] {Unit.Bits, "Bits"},
                new object[] {Unit.Kilobits, "Kilobits"},
                new object[] {Unit.Megabits, "Megabits"},
                new object[] {Unit.Gigabits, "Gigabits"},
                new object[] {Unit.Terabits, "Terabits"},
                new object[] {Unit.Percent, "Percent"},
                new object[] {Unit.Count, "Count"},
                new object[] {Unit.BytesPerSecond, "Bytes/Second"},
                new object[] {Unit.KilobytesPerSecond, "Kilobytes/Second"},
                new object[] {Unit.MegabytesPerSecond, "Megabytes/Second"},
                new object[] {Unit.GigabytesPerSecond, "Gigabytes/Second"},
                new object[] {Unit.TerabytesPerSecond, "Terabytes/Second"},
                new object[] {Unit.BitsPerSecond, "Bits/Second"},
                new object[] {Unit.KilobitsPerSecond, "Kilobits/Second"},
                new object[] {Unit.MegabitsPerSecond, "Megabits/Second"},
                new object[] {Unit.GigabitsPerSecond, "Gigabits/Second"},
                new object[] {Unit.TerabitsPerSecond, "Terabits/Second"},
                new object[] {Unit.CountPerSecond, "Count/Second"},
                new object[] {Unit.None, "None"}
            };
    }
}
