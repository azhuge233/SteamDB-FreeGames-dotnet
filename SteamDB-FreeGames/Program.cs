using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;

namespace SteamDB_FreeGames {
	class Program {
		private static readonly string SteamDBUrl = "https://steamdb.info/upcoming/free/";
		private static readonly string configPath = "config.json";
		private static readonly string recordPath = "record.json";

		static async Task SendNotification(List<string> msgs) {
			if (msgs.Count() == 0) {
				Console.WriteLine("No new notifications !");
				return ;
			}

			using var jsonOp = new JsonOP();
			var config = new Dictionary<string, string>();
			try {
				config = jsonOp.LoadConfig(path: configPath);
			} catch (Exception e) {
				Console.WriteLine("\nError loading config file !");
				Console.WriteLine("Error message: {0}", e.Message);
				throw e;
			}

			using var myBot = new TgBot(config["TOKEN"]);
			int count = 1;
			foreach (var msg in msgs) {
				Console.WriteLine("Sending Message {0}", count++);
				await myBot.SendMessage(
					chatId: config["CHAT_ID"],
					msg: msg,
					htmlMode: true
				);
			}
		}

		static async Task Main() {
			var pushList = new List<string>();
			var recordList = new List<Dictionary<string, string>>();

			using (var jsonOp = new JsonOP()) {
				var records = new List<Dictionary<string, string>>();
				try {
					Console.WriteLine("Loading previous records...");
					records = jsonOp.LoadData(recordPath);// load previous free game info
				} catch (Exception e) {
					Console.WriteLine("\nError loading previous records !");
					Console.WriteLine("Error message: {0}\n", e.Message);
					throw e;
				}
				var htmlDoc = new HtmlDocument(); 

				Console.WriteLine("Getting page source...");
				try {
					using var getSource = new GetSourceClass();
					// selenium get page source
					var html = getSource.getSource(SteamDBUrl);
					// parse source string to HtmlAgilityPack
					htmlDoc.LoadHtml(html);
				} catch (Exception e) {
					Console.WriteLine("\nError getting page source !");
					Console.WriteLine("Error message: {0}\n", e.Message);
					throw e;
				}

				Console.WriteLine("Finding free games...\n");
				var apps = htmlDoc.DocumentNode.CssSelect("table tr.app"); //find all free games
				foreach (var each in apps) {
					var tds = each.CssSelect("td").ToArray();
					var tdLen = tds.Count(); //steamDB added an extra column with a intall button

					//start gather free game basic info
					string subID = tds[1].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('/')[2];
					string gameName = tds[1].SelectSingleNode(".//b").InnerText;
					string gameURL = tds[0].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('?')[0];
					string freeType = tdLen == 5 ? tds[2].InnerHtml.ToString() : tds[3].InnerHtml.ToString(); //steamDB added an extra column with a intall button
					DateTime startTime = DateTime.ParseExact(tdLen == 5 ? tds[3].Attributes["title"].Value.ToString() : tds[4].Attributes["title"].Value.ToString(), "d MMMM yyyy – HH:mm:ss UTC", System.Globalization.CultureInfo.InvariantCulture).AddHours(8); //steamDB added an extra column with a intall button
					DateTime endTime = DateTime.ParseExact(tdLen == 5 ? tds[4].Attributes["title"].Value.ToString() : tds[5].Attributes["title"].Value.ToString(), "d MMMM yyyy – HH:mm:ss UTC", System.Globalization.CultureInfo.InvariantCulture).AddHours(8); //steamDB added an extra column with a intall button

					if (freeType != "Weekend") {
						Console.WriteLine("Found free game: {0}", gameName);
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
							var browser = new ScrapingBrowser() { Encoding = Encoding.UTF8 };
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
							Console.WriteLine("Added game {0} in push list", gameName);
						}
					}
				}

				Console.WriteLine("\nWriting records...");
				//write new record
				if (recordList.Count > 0) {
					jsonOp.WriteData(recordList, recordPath);
					Console.WriteLine("Done writing records !");
				} else
					Console.WriteLine("No records detected, quit writing records...");
			}
			Console.WriteLine("Sending notification...");
			await SendNotification(pushList);
			Console.WriteLine("Task done!");
		}
	}
}
