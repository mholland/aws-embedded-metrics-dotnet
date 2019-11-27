using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AWSEmbeddedMetrics.Tests
{
    public sealed class MetricLoggerExtensionTests : IDisposable
    {
        private readonly TextWriter _stdOut;
        private readonly StringWriter _logWriter;
        private readonly StringBuilder _logOutput = new StringBuilder();

        public MetricLoggerExtensionTests()
        {
            _logOutput.Clear();
            _logWriter = new StringWriter(_logOutput);
            _stdOut = Console.Out;
            Console.SetOut(_logWriter);
        }

        public void Dispose()
        {
            Console.SetOut(_stdOut);
            _logWriter.Dispose();
        }

        [Fact]
        public void WhenNoOptionsAreSpecifiedThenTheDefaultOptionsAreUsed()
        {
            var sc = new ServiceCollection().AddMetricLogger().BuildServiceProvider();
            
            sc.GetService<IMetricLogger>()
                .Should()
                .NotBeNull()
                .And
                .BeOfType<MetricLogger>()
                .Which
                .Log(MetricBuilder.Create().SpecifyMetric("foo", 42.0, Unit.CountPerSecond).Build());

            JObject
                .Parse(_logOutput.ToString())
                .SelectToken("_aws.CloudWatchMetrics[0].Namespace")
                .ToObject<string>()
                .Should()
                .Be("aws-embedded-metrics");
        }

        [Fact]
        public void WhenOptionsAreProvidedThenTheDefaultsAreOverriden()
        {
            var sc = new ServiceCollection()
                .AddMetricLogger(new MetricLoggerOptions{DefaultNamespace = "Overriden-Namespace"})
                .BuildServiceProvider();

            sc.GetService<IMetricLogger>()
                .Should()
                .NotBeNull()
                .And
                .BeOfType<MetricLogger>()
                .Which
                .Log(MetricBuilder.Create().SpecifyMetric("foo", 42).Build());

            JObject
                .Parse(_logOutput.ToString())
                .SelectToken("_aws.CloudWatchMetrics[0].Namespace")
                .ToObject<string>()
                .Should()
                .Be("Overriden-Namespace");
        }

        [Fact]
        public void WhenOptionsAreConfiguredWithAnActionThenTheDefaultsAreOverriden()
        {
            var sc = new ServiceCollection()
                .AddMetricLogger(o => { o.DefaultNamespace = "Configured-Namespace"; })
                .BuildServiceProvider();

            sc.GetService<IMetricLogger>()
                .Should()
                .NotBeNull()
                .And
                .BeOfType<MetricLogger>()
                .Which
                .Log(MetricBuilder.Create().SpecifyMetric("foo", 42).Build());

            JObject
                .Parse(_logOutput.ToString())
                .SelectToken("_aws.CloudWatchMetrics[0].Namespace")
                .ToObject<string>()
                .Should()
                .Be("Configured-Namespace");
        }
    }
}
