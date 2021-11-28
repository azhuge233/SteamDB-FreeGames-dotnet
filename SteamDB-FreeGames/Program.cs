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
                var servicesProvider = DI.BuildDiAll();

                logger.Info(" - Start Job -");

                using (servicesProvider as IDisposable) {
                    var jsonOp = servicesProvider.GetRequiredService<JsonOP>();

                    var config = jsonOp.LoadConfig();
                    var oldRecord = jsonOp.LoadData();
                    servicesProvider.GetRequiredService<ConfigValidator>().CheckValid(config);

                    // Get page source
                    var source = await servicesProvider.GetRequiredService<Scraper>().GetSteamDBSource(config);
                    //var source = System.IO.File.ReadAllText("test.html");

                    // Parse page source
                    var parseResult = servicesProvider.GetRequiredService<Parser>().HtmlParse(source, oldRecord);

                    // Notify first, then write records
                    await servicesProvider.GetRequiredService<NotifyOP>().Notify(config, oldRecord, config.NotifyKeepGamesOnly ? parseResult.PushListKeepOnly : parseResult.PushListAll);

                    // Write new records
                    jsonOp.WriteData(parseResult.Records);
                }

                logger.Info(" - Job End -\n");
            } catch (Exception ex) {
                logger.Error(ex.Message);
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
