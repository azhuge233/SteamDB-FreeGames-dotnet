using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames {
	class Parser : IDisposable {
		#region DI variable
		private readonly ILogger<Parser> _logger;
		private readonly IServiceProvider services = Program.BuildDi();
		#endregion

		#region configKeys string
		public readonly string useHeadlessKey = "ENABLE_HEADLESS";
		public readonly string keepGamesOnlyKey = "KEEP_GAMES_ONLY";
		#endregion

		#region debug strings
		private readonly string debugHtmlParser = "Parse";
		private readonly string debugConfigConvertToBool = "Convert config files to bool";
		#endregion

		private readonly string SteamDBDateFormat = "yyyy-MM-dTHH:mm:ss+00:00";

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public Tuple<List<FreeGameRecord>, List<FreeGameRecord>> HtmlParse(string source, List<FreeGameRecord> records, bool keepGamesOnly = true) {
			try {
				_logger.LogDebug(debugHtmlParser);

				var pushList = new List<FreeGameRecord>(); // notification list
				var recordList = new List<FreeGameRecord>(); // new records list
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var apps = htmlDoc.DocumentNode.CssSelect("table tr.app");
				foreach (var each in apps) {
					//skip the hidden trap row
					if (each.Attributes.HasKeyIgnoreCase("hidden")) continue;

					var tds = each.CssSelect("td").ToArray();

					var newFreeGame = new FreeGameRecord {
						//start gather free game basic info
						SubID = tds[1].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('/')[2],
						Name = tds[1].SelectSingleNode(".//b").InnerText,
						FreeType = tds[3].InnerHtml.ToString() == "Weekend" ? "Weekend" : "Keep",
						Url = tds[0].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('?')[0],
						StartTime = tds[4].Attributes["data-time"] == null ? DateTime.Now : DateTime.ParseExact(tds[4].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8), // in case of blank start/end time
						EndTime = tds[5].Attributes["data-time"] == null ? DateTime.Now : DateTime.ParseExact(tds[5].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8)
					};

					if (keepGamesOnly) {

						_logger.LogDebug("Found game: {0}. Freetype: {1}", newFreeGame.Name, newFreeGame.FreeType);

						if (newFreeGame.FreeType == "Keep") {
							_logger.LogInformation("Found free game: {0}", newFreeGame.Name);

							//add game info to recordList
							recordList.Add(newFreeGame);

							if (!records.Where(x => x.SubID == newFreeGame.SubID).Any()) { // the game is not in the previous record(a new game)
																						   // try to get game name on Steam page 
								var tmpDoc = services.GetRequiredService<Scraper>().GetSteamSource(newFreeGame.Url);

								var steamName = tmpDoc.DocumentNode.CssSelect("div.apphub_AppName").ToArray();
								if (steamName.Length > 0)
									newFreeGame.Name = steamName[0].InnerText;

								pushList.Add(newFreeGame);
								_logger.LogInformation("Added game {0} in push list", newFreeGame.Name);
							} else {
								_logger.LogInformation("{0} is found in previous records, stop adding in push list", newFreeGame.Name);
							}
						}
					} else {
						_logger.LogInformation("Found game: {0}. Freetype: {1}", newFreeGame.Name, newFreeGame.FreeType);

						recordList.Add(newFreeGame);

						if (!records.Where(x => x.SubID == newFreeGame.SubID).Any()) { // the game is not in the previous record(a new game)
																					   // try to get game name on Steam page 
							var tmpDoc = services.GetRequiredService<Scraper>().GetSteamSource(newFreeGame.Url);

							var steamName = tmpDoc.DocumentNode.CssSelect("div.apphub_AppName").ToArray();
							if (steamName.Length > 0)
								newFreeGame.Name = steamName[0].InnerText;

							pushList.Add(newFreeGame);
							_logger.LogInformation("Added game {0} in push list", newFreeGame.Name);
						} else {
							_logger.LogInformation("{0} is found in previous records, stop adding in push list", newFreeGame.Name);
						}
					}
				}

				_logger.LogDebug($"Done: {debugHtmlParser}");
				return new Tuple<List<FreeGameRecord>, List<FreeGameRecord>>(pushList, recordList);
			} catch (Exception) {
				_logger.LogError($"Error: {debugHtmlParser}");
				throw;
			}
		}

		public Dictionary<string, bool> ConvertConfigToBool(Dictionary<string, string> config) {
			try {
				_logger.LogDebug(debugConfigConvertToBool);
				var dic = new Dictionary<string, bool> {
					{ useHeadlessKey, Convert.ToBoolean(config[useHeadlessKey]) },
					{ keepGamesOnlyKey, Convert.ToBoolean(config[keepGamesOnlyKey]) }
				};
				_logger.LogDebug($"Done: {debugConfigConvertToBool}");
				return dic;
			} catch (Exception) {
				_logger.LogError($"Error: {debugConfigConvertToBool}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
