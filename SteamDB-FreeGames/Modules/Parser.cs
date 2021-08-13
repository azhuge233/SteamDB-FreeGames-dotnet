using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames {
	class Parser : IDisposable {
		#region DI variable
		private readonly ILogger<Parser> _logger;
		#endregion

		#region debug strings
		private readonly string debugHtmlParser = "Parse";
		private readonly string infoGameFound = "Found game: {0}. Freetype: {1}";
		private readonly string infoAddToPushListKeepOnly = "Added game {0} to keep only push list";
		private readonly string infoAddToPushListAll = "Added game {0} to all push list";
		private readonly string infoFoundInPreviousRecords = "{0} is found in previous records, stop adding in push list";
		#endregion

		#region XPath strings
		private readonly string XPathRecord = ".//div[@class=\'container\']//table//tr[@class=\'app\']";
		private readonly string XPathtds = ".//td";
		#endregion

		#region parser stings
		private readonly string SteamDBDateFormat = "yyyy-MM-dTHH:mm:ss+00:00";
		private readonly string keepGameString = "Keep";
		private readonly string hiddenAttribute = "hidden";
		#endregion

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public ParseResult HtmlParse(string source, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugHtmlParser);

				var parseResult = new ParseResult();
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var apps = htmlDoc.DocumentNode.SelectNodes(XPathRecord);
				
				foreach (var each in apps) {
					//skip the hidden trap row
					if (each.Attributes.Contains(hiddenAttribute)) continue;

					var tds = each.SelectNodes(XPathtds).ToList();

					var newFreeGame = new FreeGameRecord {
						//start gather free game basic info
						//SubID change to ID, SteamDB does not always provide game's SubID, somtimes AppID
						ID = tds[1].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Trim('/'),
						Name = tds[1].SelectSingleNode(".//b").InnerText,
						FreeType = tds[3].InnerHtml.ToString() == "Weekend" ? "Weekend" : "Keep",
						Url = tds[0].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('?')[0],
						StartTime = tds[4].Attributes["data-time"] == null ? DateTime.Now : DateTime.ParseExact(tds[4].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8), // in case of blank start/end time
						EndTime = tds[5].Attributes["data-time"] == null ? DateTime.Now : DateTime.ParseExact(tds[5].Attributes["data-time"].Value.ToString(), SteamDBDateFormat, System.Globalization.CultureInfo.InvariantCulture).AddHours(8)
					};

					_logger.LogInformation(infoGameFound, newFreeGame.Name, newFreeGame.FreeType);

					//add game info to recordList
					parseResult.Records.Add(newFreeGame);

					// the game is not in the previous record
					if (records.Count == 0 || !records.Exists(record => record.Name == newFreeGame.Name && record.ID == newFreeGame.ID && record.FreeType == newFreeGame.FreeType && record.StartTime == newFreeGame.StartTime && record.EndTime == newFreeGame.EndTime )) {
						if (newFreeGame.FreeType == keepGameString) {
							parseResult.PushListKeepOnly.Add(newFreeGame);
							_logger.LogInformation(infoAddToPushListKeepOnly, newFreeGame.Name);
						}
						parseResult.PushListAll.Add(newFreeGame);
						_logger.LogInformation(infoAddToPushListAll, newFreeGame.Name);
					} else _logger.LogInformation(infoFoundInPreviousRecords, newFreeGame.Name);
				}

				_logger.LogDebug($"Done: {debugHtmlParser}");
				return parseResult;
			} catch (Exception) {
				_logger.LogError($"Error: {debugHtmlParser}");
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
