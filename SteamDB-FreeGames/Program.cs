using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

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
                    var playwrightOp = servicesProvider.GetRequiredService<Scraper>();
                    var tgBot = servicesProvider.GetRequiredService<TgBot>();
                    var parser = servicesProvider.GetRequiredService<Parser>();

                    var config = jsonOp.LoadConfig();
                    var convertedBools = parser.ConvertConfigToBool(config);

                    // Get page source
                    var source = await playwrightOp.GetSteamDBSource(convertedBools[parser.useHeadlessKey]);

                    // Parse page source
                    var parseResult = parser.HtmlParse(source, jsonOp.LoadData(convertedBools[parser.keepGamesOnlyKey]), convertedBools[parser.keepGamesOnlyKey]);
                    var pushList = parseResult.Item1; // notification list
                    var recordList = parseResult.Item2; // new records list

                    // Write new records
                    jsonOp.WriteData(recordList, convertedBools[parser.keepGamesOnlyKey]);

                    //Send notifications
                    await tgBot.SendMessage(token: config["TOKEN"], chatID: config["CHAT_ID"], pushList, htmlMode: true);
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
