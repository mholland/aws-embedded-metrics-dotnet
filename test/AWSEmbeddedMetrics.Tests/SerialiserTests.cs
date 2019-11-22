using System;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AWSEmbeddedMetrics.Tests
{
    public sealed class SerialiserTests
    {
        [Fact]
        public void ThenTheSerialisedMetricIsFormattedCorrectly()
        {
            var serialiser = new Serialiser(new TestClock(new DateTimeOffset(2015, 09, 28, 09, 00, 00, TimeSpan.Zero)));
            var metric = MetricBuilder
                .Create()
                .SpecifyMetric("time", 100, Unit.Milliseconds)
                .AddDimension("functionVersion", "$LATEST")
                .AddDimension("functionName", "LambdaFunction")
                .AddProperty("requestId", "12345678-9ace-4817-a57c-e4dd734019ee")
                .AddProperty("requestPath", "/foo/bar")
                .WithNamespace("lambda-function-metrics")
                .Build();

            var json = serialiser.SerialiseMetric(metric);
            JObject
                .Parse(json)
                .Should()
                .BeEquivalentTo(@"
{
    ""_aws"": {
        ""Timestamp"": 1443430800,
        ""CloudWatchMetrics"": [
            {
                ""Namespace"": ""lambda-function-metrics"",
                ""Dimensions"": [
                    [
                        ""functionVersion"",
                        ""functionName""
                    ]
                ],
                ""Metrics"": [
                    {
                        ""Name"": ""time"",
                        ""Unit"": ""Milliseconds""
                    }
                ]
            }
        ]
    },
    ""functionVersion"": ""$LATEST"",
    ""functionName"": ""LambdaFunction"",
    ""time"": 100,
    ""requestId"": ""12345678-9ace-4817-a57c-e4dd734019ee"",
    ""requestPath"": ""/foo/bar""
}
");
        }

        [Fact]
        public void WhenMoreThanTenDimensionsAreProvidedThenOnlyTenAreRecorded()
        {
            var serialiser = new Serialiser(new TestClock());
            var metric = MetricBuilder
                .Create()
                .SpecifyMetric("time", 100, Unit.Milliseconds)
                .AddDimension("One", "1")
                .AddDimension("Two", "2")
                .AddDimension("Three", "3")
                .AddDimension("Four", "4")
                .AddDimension("Five", "5")
                .AddDimension("Six", "6")
                .AddDimension("Seven", "7")
                .AddDimension("Eight", "8")
                .AddDimension("Nine", "9")
                .AddDimension("Ten", "10")
                .AddDimension("Eleven", "11")
                .AddDimension("Twelve", "12")
                .AddDimension("Thirteen", "13")
                .Build();

            var json = serialiser.SerialiseMetric(metric);

            JObject
                .Parse(json)
                .SelectToken("$._aws.CloudWatchMetrics[0].Dimensions[0]")
                .ToObject<string[]>()
                .Should()
                .HaveCount(10, "CloudWatch accepts a maximum of 10 dimensions.");
        }
    }
}
