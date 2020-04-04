using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;

namespace SteamDB_FreeGames {
	class Program {
		private static string SteamDBUrl = "https://steamdb.info/upcoming/free/";
		private static string configPath = "config.json";
		private static string recordPath = "record.json";

		static void SendNotification(List<string> msgs) {
			using (var jsonOp = new JsonOP()) {
				var config = jsonOp.LoadConfig(path: configPath);
				using (var myBot = new TgBot(config["TOKEN"])) {
					foreach (var msg in msgs) {
						myBot.SendMessage(
							chatId: config["CHAT_ID"],
							msg: msg,
							htmlMode: true
						);
						Thread.Sleep(2000);
					}
				}
			}
		}

		static void Main() {
			var pushList = new List<string>();
			var recordList = new List<Dictionary<string, string>>();

			using (var jsonOp = new JsonOP()) {
				var records = jsonOp.LoadData(recordPath); // load previous free game info
				var htmlDoc = new HtmlDocument();

				using (var getSource = new GetSourceClass()) {
					var html = getSource.getSource(SteamDBUrl); //selenium get page source

					htmlDoc.LoadHtml(html); // parse source string to HtmlAgilityPack
				}

				var apps = htmlDoc.DocumentNode.CssSelect("table tr.app"); //find all free games
				foreach (var each in apps) {
					var tds = each.CssSelect("td").ToArray();
					if (tds[2].InnerHtml.ToString() != "Weekend") {

						//start gather free game basic info
						string subID = tds[1].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('/')[2];
						string gameName = tds[1].SelectSingleNode(".//b").InnerText;
						string gameURL = tds[0].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('?')[0];
						DateTime startTime = DateTime.ParseExact(tds[3].Attributes["title"].Value.ToString(), "d MMMM yyyy – HH:mm:ss UTC", System.Globalization.CultureInfo.InvariantCulture).AddHours(8);
						DateTime endTime = DateTime.ParseExact(tds[4].Attributes["title"].Value.ToString(), "d MMMM yyyy – HH:mm:ss UTC", System.Globalization.CultureInfo.InvariantCulture).AddHours(8);

						//add game info to recordList
						var tmpDic = new Dictionary<string, string> {
							["Name"] = gameName,
							["SubID"] = subID,
							["URL"] = gameURL,
							["StartTime"] = startTime.ToString(),
							["EndTime"] = endTime.ToString()
						};
						recordList.Add(tmpDic);

						//decide to send notification
						bool is_push = true;
						foreach (var record in records) {
							if (record["SubID"] == subID) {
								is_push = false;
								break;
							}
						}

						if (is_push) { //the game is not in the previous record(a new game)
									   //try to get game name on Steam page 
							var browser = new ScrapingBrowser();
							WebPage page = browser.NavigateToPage(new Uri(gameURL));
							var tmpDoc = new HtmlDocument();
							tmpDoc.LoadHtml(page.Content);
							var steamName = tmpDoc.DocumentNode.CssSelect("div.apphub_AppName").ToArray();
							if (steamName.Count() > 0)
								gameName = steamName[0].InnerText;

							string pushMessage = "<b>" + gameName + "</b> \n\n";
							pushMessage += "Sub ID: <i>" + subID + "</i> \n";
							pushMessage += "链接: <a href=\"" + gameURL + "\" >" + gameName + "</a>\n";
							pushMessage += "开始时间: " + startTime.ToString() + "\n";
							pushMessage += "结束时间: " + endTime.ToString() + "\n";

							pushList.Add(pushMessage);
						}
					}
				}
				jsonOp.WriteData(recordList, recordPath); //write new record
			}
			SendNotification(pushList);
		}
	}
}
