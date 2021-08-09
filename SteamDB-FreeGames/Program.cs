using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SteamDB_FreeGames.Notifier;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames {
    class Program {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
                   .SetBasePath(System.IO.Directory.GetCurrentDirectory())
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
                    var convertedBools = parser.ConvertConfigToBool(config);
                    var convertedInts = parser.ConvertConfigToInt(config);

                    // Get page source
                    var source = await servicesProvider.GetRequiredService<Scraper>().GetSteamDBSource(convertedInts[ConfigKeys.TimeOutSecKey], convertedBools[ConfigKeys.UseHeadlessKey]);
                    //var source = System.IO.File.ReadAllText("test.html");

                    // Parse page source
                    var parseResult = parser.HtmlParse(source, jsonOp.LoadData(convertedBools[ConfigKeys.KeepGamesOnlyKey]), convertedBools[ConfigKeys.KeepGamesOnlyKey]);
                    var pushList = parseResult.Item1; // notification list
                    var recordList = parseResult.Item2; // new records list
                    
                    //Notify first, then write records
                    // Telegram notifications
                    if(convertedBools[ConfigKeys.EnableTelegramKey])
                        await servicesProvider.GetRequiredService<TgBot>().SendMessage(token: config[ConfigKeys.TelegramTokenKey], chatID: config[ConfigKeys.TelegramChatIDKey], pushList, htmlMode: true);

                    // Bark notifications
                    if(convertedBools[ConfigKeys.EnableBarkKey])
                        await servicesProvider.GetRequiredService<Barker>().SendMessage(config[ConfigKeys.BarkAddressKey], config[ConfigKeys.BarkTokenKey], pushList);

                    // Write new records
                    jsonOp.WriteData(recordList, convertedBools[ConfigKeys.KeepGamesOnlyKey]);
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
