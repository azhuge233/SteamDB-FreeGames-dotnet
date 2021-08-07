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

        private static IServiceProvider BuildDi() {
            return new ServiceCollection()
               .AddTransient<PlayWrightOP>()
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
                    // Get telegram bot token, chatID and previous records
                    var jsonOp = servicesProvider.GetRequiredService<JsonOP>();
                    var config = jsonOp.LoadConfig(); // token, chatID
                    var oldRecords = jsonOp.LoadData(); // old records

                    // Get page source
                    var playwrightOp = servicesProvider.GetRequiredService<PlayWrightOP>();
                    var source = playwrightOp.GetHtmlSource(Convert.ToBoolean(config["ENABLE_HEADLESS"]));

                    // Parse page source
                    var parser = servicesProvider.GetRequiredService<Parser>();
                    var parseResult = parser.Parse(source.Result, oldRecords);
                    var pushList = parseResult.Item1; // notification list
                    var recordList = parseResult.Item2; // new records list

                    // Write new records
                    jsonOp.WriteData(recordList);

                    //Send notifications
                    var tgBot = servicesProvider.GetRequiredService<TgBot>();
                    await tgBot.SendMessage(token: config["TOKEN"], chatID: config["CHAT_ID"], pushList, htmlMode: true);
                }

                logger.Info(" - Job End -\n");
            } catch (Exception ex) {
                logger.Error(ex.Message);
                logger.Error(ex.InnerException.Message);
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
