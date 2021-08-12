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
        public static IServiceProvider BuildDi() {
            return new ServiceCollection()
               .AddTransient<Scraper>()
               .AddTransient<Parser>()
               .AddTransient<TgBot>()
               .AddTransient<JsonOP>()
               .AddTransient<Barker>()
               .AddTransient<NotifyOP>()
               .AddTransient<ConfigValidator>()
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
