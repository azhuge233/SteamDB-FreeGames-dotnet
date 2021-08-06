using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;
		private readonly string SteamDBDateFormat = "yyyy-MM-dTHH:mm:ss+00:00";

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public Tuple<List<string>, List<Dictionary<string, string>>> Parse(string source, List<Dictionary<string, string>> records) {
			try {
				var pushList = new List<string>(); // notification list
				var recordList = new List<Dictionary<string, string>>(); // new records list
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var apps = htmlDoc.DocumentNode.CssSelect("table tr.app");
				foreach (var each in apps) {
					//skip the hidden trap row
					if (each.Attributes.HasKeyIgnoreCase("hidden")) continue;

					var tds = each.CssSelect("td").ToArray();
					var tdLen = tds.Length; //steamDB added an extra column with a intall button

					//start gather free game basic info
					string subID = tds[1].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('/')[2];
					string gameName = tds[1].SelectSingleNode(".//b").InnerText;
					string gameURL = tds[0].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('?')[0];
					string freeType = tdLen == 5 ? tds[2].InnerHtml.ToString() : tds[3].InnerHtml.ToString(); //steamDB added an extra column with a intall button

					string startTime, endTime;

					if (tdLen == 5) { //steamDB added an extra column with a intall button
						startTime = tds[3].Attributes["data-time"] == null ? "None" : DateTime.ParseExact(tds[3].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8).ToString(); // in case of blank start time or end time
						endTime = tds[4].Attributes["data-time"] == null ? "None" : DateTime.ParseExact(tds[4].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8).ToString();
					} else {
						startTime = tds[4].Attributes["data-time"] == null ? "None" : DateTime.ParseExact(tds[4].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8).ToString();
						endTime = tds[5].Attributes["data-time"] == null ? "None" : DateTime.ParseExact(tds[5].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8).ToString();
					}

					_logger.LogDebug("Found game: {0}. Freetype: {1}", gameName, freeType);

					if (freeType != "Weekend") {
						_logger.LogInformation("Found free game: {0}", gameName);
						//add game info to recordList
						var tmpDic = new Dictionary<string, string> {
						{ "Name", gameName }, { "SubID", subID }, { "URL", gameURL },
						{ "StartTime", startTime }, { "EndTime", endTime }
					};
						recordList.Add(tmpDic);

						if (!records.Where(x => x["SubID"] == subID).Any()) { // the game is not in the previous record(a new game)
																			  // try to get game name on Steam page 
							var browser = new ScrapingBrowser() { Encoding = Encoding.UTF8 };
							WebPage page = browser.NavigateToPage(new Uri(gameURL));
							var tmpDoc = new HtmlDocument();
							tmpDoc.LoadHtml(page.Content);
							var steamName = tmpDoc.DocumentNode.CssSelect("div.apphub_AppName").ToArray();
							if (steamName.Length > 0)
								gameName = steamName[0].InnerText;

							StringBuilder pushMessage = new();
							pushMessage.AppendFormat("<b>{0}</b>\n\nSub ID: <i>{1}</i>\n链接: <a href=\"{2}\" > {3}</a>\n开始时间: {4}\n结束时间: {5}\n", gameName, subID, gameURL, gameName, startTime, endTime);

							pushList.Add(pushMessage.ToString());
							_logger.LogInformation("Added game {0} in push list", gameName);
						} else {
							_logger.LogInformation("{0} is found in previous records, stop adding in push list", gameName);
						}
					}
				}

				return new Tuple<List<string>, List<Dictionary<string, string>>>(pushList, recordList);
			} catch (Exception) {
				_logger.LogError("Parsing Error");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
