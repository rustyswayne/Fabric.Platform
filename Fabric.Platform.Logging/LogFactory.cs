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
            string clientName, string applicationName, bool writeToFile = false)
        {
            var logPrefix = $"logstash-{clientName}-{applicationName}";
            var sinkOptions = new ElasticsearchSinkOptions(elasticSearchSettings.GetElasticSearchUri())
            {
                IndexFormat = logPrefix + "-{0:yyyy.MM.dd}"
            };
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .WriteTo.Elasticsearch(sinkOptions);

            if (writeToFile)
            {
                loggerConfiguration.WriteTo.RollingFile("Logs\\log-{Date}.txt");
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}
