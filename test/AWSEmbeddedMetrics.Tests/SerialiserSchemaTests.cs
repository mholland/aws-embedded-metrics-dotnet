using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NJsonSchema;
using NJsonSchema.Validation;
using Xunit;

namespace AWSEmbeddedMetrics.Tests
{
    public sealed class SerialiserSchemaTests
    {
        private static string _embeddedFormatSchema = @"{
  ""definitions"": {},
  ""$schema"": """",
  ""$id"": ""http://example.com/root.json"",
  ""type"": ""object"",
  ""title"": ""Root Node"",
  ""required"": [""_aws""],
  ""properties"": {
    ""_aws"": {
      ""$id"": ""#/properties/_aws"",
      ""type"": ""object"",
      ""title"": ""Metadata"",
      ""required"": [""Timestamp"", ""CloudWatchMetrics""],
      ""properties"": {
        ""Timestamp"": {
          ""$id"": ""#/properties/_aws/properties/Timestamp"",
          ""type"": ""integer"",
          ""title"": ""The Timestamp Schema"",
          ""examples"": [1565375354953]
        },
        ""CloudWatchMetrics"": {
          ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics"",
          ""type"": ""array"",
          ""title"": ""MetricDirectives"",
          ""items"": {
            ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items"",
            ""type"": ""object"",
            ""title"": ""MetricDirective"",
            ""required"": [""Namespace"", ""Dimensions"", ""Metrics""],
            ""properties"": {
              ""Namespace"": {
                ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Namespace"",
                ""type"": ""string"",
                ""title"": ""CloudWatch Metrics Namespace"",
                ""examples"": [""MyApp""],
                ""pattern"": ""^(.*)$""
              },
              ""Dimensions"": {
                ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Dimensions"",
                ""type"": ""array"",
                ""title"": ""The Dimensions Schema"",
                ""items"": {
                  ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Dimensions/items"",
                  ""type"": ""array"",
                  ""title"": ""DimensionSet"",
                  ""items"": {
                    ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Dimensions/items/items"",
                    ""type"": ""string"",
                    ""title"": ""DimensionReference"",
                    ""examples"": [""Operation""],
                    ""pattern"": ""^(.*)$""
                  }
                }
              },
              ""Metrics"": {
                ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Metrics"",
                ""type"": ""array"",
                ""title"": ""MetricDefinitions"",
                ""items"": {
                  ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Metrics/items"",
                  ""type"": ""object"",
                  ""title"": ""MetricDefinition"",
                  ""required"": [""Name"", ""Unit""],
                  ""properties"": {
                    ""Name"": {
                      ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Metrics/items/properties/Name"",
                      ""type"": ""string"",
                      ""title"": ""MetricName"",
                      ""examples"": [""ProcessingLatency""],
                      ""pattern"": ""^(.*)$""
                    },
                    ""Unit"": {
                      ""$id"": ""#/properties/_aws/properties/CloudWatchMetrics/items/properties/Metrics/items/properties/Unit"",
                      ""type"": ""string"",
                      ""title"": ""MetricUnit"",
                      ""examples"": [""Milliseconds""],
                      ""pattern"": ""^(Seconds|Microseconds|Milliseconds|Bytes|Kilobytes|Megabytes|Gigabytes|Terabytes|Bits|Kilobits|Megabits|Gigabits|Terabits|Percent|Count|Bytes\\/Second|Kilobytes\\/Second|Megabytes\\/Second|Gigabytes\\/Second|Terabytes\\/Second|Bits\\/Second|Kilobits\\/Second|Megabits\\/Second|Gigabits\\/Second|Terabits\\/Second|Count\\/Second|None)$""
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}";

        public static IEnumerable<object[]> MetricTestData
            => new List<object[]>
            {
                new object[]
                {
                    MetricBuilder
                        .Create()
                        .SpecifyMetric("time", 100, Unit.Milliseconds)
                        .AddDimension("functionVersion", "$LATEST")
                        .AddProperty("requestId", Guid.NewGuid().ToString())
                        .WithNamespace("lambda-function-metrics")
                        .Build()
                },
                new object[]
                {
                    MetricBuilder
                        .Create()
                        .SpecifyMetric("time", 100)
                        .AddDimension("functionVersion", "$LATEST")
                        .AddDimension("functionName", "LambdaFunction")
                        .AddProperty("requestId", Guid.NewGuid().ToString())
                        .AddProperty("One", Guid.NewGuid().ToString())
                        .AddProperty("Two", "/foo/bar")
                        .AddProperty("Three", "Value")
                        .WithNamespace("lambda-function-metrics")
                        .Build()
                }
            };


        [Theory]
        [MemberData(nameof(MetricTestData))]
        public async Task GivenAMetricWhenSerialisedThenTheFormatValidatesSuccessfullyAgainstTheSchema(Metric metric)
        {
            var serialiser = new Serialiser(new SystemClock());

            var format = serialiser.SerialiseMetric(metric);
            var schema = await JsonSchema.FromJsonAsync(_embeddedFormatSchema);

            var validator = new JsonSchemaValidator();
            validator.Validate(format, schema).Should().BeEmpty(" there were no validation errors");
        }
    }
}
