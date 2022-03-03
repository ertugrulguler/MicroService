using Autofac;
using Framework.Core.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using System;
using Module = Autofac.Module;

namespace Catalog.Container.Modules
{
    public class LoggingModule : Module
    {
        private static string _elasticSearchApiUrl;
        private static string _environmentName;
        private static string _elasticSearchIndexName;
        private static string _loggerAppName;

        public static void AddLogging(IConfiguration configuration)
        {
            _elasticSearchApiUrl = configuration["Logging.ElasticSearch:Uri"];
            _elasticSearchIndexName = configuration["Logging.ElasticSearch:IndexName"];
            _loggerAppName = configuration["Logging.ApplicationName"];
            _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ILogger>((c, p) => new LoggerConfiguration()
                .Enrich.WithMachineName()
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.Elasticsearch(//todo enable ES logging
                    new ElasticsearchSinkOptions(new Uri(_elasticSearchApiUrl))
                    {
                        IndexFormat =
                            $"{_elasticSearchIndexName}-{_environmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}",
                        CustomFormatter = new ElasticsearchJsonFormatter()
                    })
                .Enrich.WithProperty("Environment", _environmentName)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("LoggerApplicationName", _loggerAppName)
                .CreateLogger()).SingleInstance();

            builder.RegisterType<CorrelationIdHelper>().As<ICorrelationIdHelper>().InstancePerLifetimeScope();
            builder.RegisterType<AppLogger>().As<IAppLogger>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}