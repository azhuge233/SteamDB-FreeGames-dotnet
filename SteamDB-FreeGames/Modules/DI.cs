using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using SteamDB_FreeGames.Notifier;

namespace SteamDB_FreeGames.Modules {
	public static class DI {
        private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .Build();
        public static IServiceProvider BuildDiAll() {
            return new ServiceCollection()
               .AddTransient<JsonOP>()
               .AddTransient<ConfigValidator>()
               .AddTransient<Scraper>()
               .AddTransient<Parser>()          
               .AddTransient<NotifyOP>()
               .AddTransient<Barker>()
               .AddTransient<TgBot>()
               .AddTransient<Email>()
               .AddTransient<QQPusher>()
               .AddTransient<PushPlus>()
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

        public static IServiceProvider BuildDiNotifierOnly() {
            return new ServiceCollection()
               .AddTransient<TgBot>()
               .AddTransient<Barker>()
               .AddTransient<Email>()
               .AddTransient<QQPusher>()
               .AddTransient<PushPlus>()
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

        public static IServiceProvider BuildDiScraperOnly() {
            return new ServiceCollection()
               .AddTransient<Scraper>()
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }
    }
}
