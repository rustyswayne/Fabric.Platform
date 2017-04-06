using Fabric.Platform.Shared.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace Fabric.Platform.Logging
{
    public class LogFactory
    {
        public static ILogger CreateLogger(LoggingLevelSwitch levelSwitch, ElasticSearchSettings elasticSearchSettings,
            string clientName)
        {
            var sinkOptions = new ElasticsearchSinkOptions(elasticSearchSettings.GetElasticSearchUri())
            {
                IndexFormat = "logstash-" + clientName + "-{0:yyyy.MM.dd}"
            };
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(sinkOptions).CreateLogger();
        }
    }
}
