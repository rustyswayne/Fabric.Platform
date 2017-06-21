using System;
using Fabric.Platform.Shared.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace Fabric.Platform.Logging
{
    public class LogFactory
    {
        public static ILogger CreateLogger(LoggingLevelSwitch levelSwitch, ElasticSearchSettings elasticSearchSettings,
            string clientName, string applicationName)
        {
            var logPrefix = $"logstash-{clientName}-{applicationName}";
            var sinkOptions = new ElasticsearchSinkOptions(elasticSearchSettings.GetElasticSearchUri())
            {
                IndexFormat = logPrefix + "-{0:yyyy.MM.dd}",
                BufferBaseFilename = elasticSearchSettings.BufferBaseFilename
            };

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .WriteTo.Elasticsearch(sinkOptions);

            return loggerConfiguration.CreateLogger();
        }
    }
}
