using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SteamDB_FreeGames.Notifier;

namespace SteamDB_FreeGames {
    class Program {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
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
               .AddTransient<ConfigValidator>()
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

        static async Task Main() {
            try {
                var servicesProvider = BuildDi();

                logger.Info(" - Start Job -");

                using (servicesProvider as IDisposable) {
                    var jsonOp = servicesProvider.GetRequiredService<JsonOP>();
                    var parser = servicesProvider.GetRequiredService<Parser>();

                    var config = jsonOp.LoadConfig();
                    servicesProvider.GetRequiredService<ConfigValidator>().CheckValid(config);

                    // Get page source
                    var source = await servicesProvider.GetRequiredService<Scraper>().GetSteamDBSource(config.TimeOutMilliSecond, config.EnableHeadless);
                    //var source = File.ReadAllText("test.html");

                    // Parse page source
                    var parseResult = parser.HtmlParse(source, jsonOp.LoadData(config.KeepGamesOnly), config.KeepGamesOnly);
                    var pushList = parseResult.Item1; // notification list
                    var recordList = parseResult.Item2; // new records list
                    
                    //Notify first, then write records
                    // Telegram notifications
                    if(config.EnableTelegram)
                        await servicesProvider.GetRequiredService<TgBot>().SendMessage(token: config.TelegramToken, chatID: config.TelegramChatID, pushList, htmlMode: true);

                    // Bark notifications
                    if(config.EnableBark)
                        await servicesProvider.GetRequiredService<Barker>().SendMessage(config.BarkAddress, config.BarkToken, pushList);

                    // Write new records
                    jsonOp.WriteData(recordList, config.KeepGamesOnly);
                }

                logger.Info(" - Job End -\n");
            } catch (Exception ex) {
                logger.Error(ex.Message);
                logger.Error($"{ex.InnerException.Message}\n\n");
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
