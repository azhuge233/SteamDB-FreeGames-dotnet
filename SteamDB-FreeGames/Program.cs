using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using SteamDB_FreeGames.Modules;

namespace SteamDB_FreeGames {
    class Program {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static async Task Main() {
            try {
                var servicesProvider = DI.BuildDi();

                logger.Info(" - Start Job -");

                using (servicesProvider as IDisposable) {
                    var jsonOp = servicesProvider.GetRequiredService<JsonOP>();

                    var config = jsonOp.LoadConfig();
                    servicesProvider.GetRequiredService<ConfigValidator>().CheckValid(config);

                    // Get page source
                    var source = await servicesProvider.GetRequiredService<Scraper>().GetSteamDBSource(config.TimeOutMilliSecond, config.EnableHeadless);
                    //var source = System.IO.File.ReadAllText("test.html");

                    // Parse page source
                    var parseResult = servicesProvider.GetRequiredService<Parser>().HtmlParse(source, jsonOp.LoadData(config.KeepGamesOnly), config.KeepGamesOnly);

                    //Notify first, then write records
                    await servicesProvider.GetRequiredService<NotifyOP>().Notify(config, parseResult.Item1);

                    // Write new records
                    jsonOp.WriteData(parseResult.Item2, config.KeepGamesOnly);
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
