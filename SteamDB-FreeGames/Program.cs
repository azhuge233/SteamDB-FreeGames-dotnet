using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using PlaywrightSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	class Program {
		#region DI variables
		private readonly ILogger _logger;
		#endregion

		#region global readonly variables
		private readonly string SteamDBUrl = "https://steamdb.info/upcoming/free/";
		private readonly string configPath = "config.json";
		private readonly string recordPath = "record.json";
		private readonly int firstDelay = 10000;
		#endregion

		public Program(ILogger<Program> logger) {
			_logger = logger;
		}

		private static void ConfigureServices(ServiceCollection services) {
			services.AddLogging(logging => {
				//logging.AddFilter("Program", LogLevel.Debug);
				logging.AddFilter("System", LogLevel.Warning);
				logging.AddFilter("Microsoft", LogLevel.Warning);
				logging.AddSimpleConsole(options => {
					options.SingleLine = true;
					options.TimestampFormat = "yyyy/MM/dd - HH:mm:ss ";
				});
			}).AddTransient<Program>();
		}

		internal async Task SendNotification(string id, string token, List<string> msgs) {
			if (msgs.Count == 0) {
				_logger.LogInformation("No new notifications !");
				return;
			}

			try {
				using var myBot = new TgBot(token);
				int count = 1;
				foreach (var msg in msgs) {
					_logger.LogInformation("Sending Message {0}", count++);
					await myBot.SendMessage(
						chatId: id,
						msg: msg,
						htmlMode: true
					);
				}
			} catch (Exception ex) {
				_logger.LogError("Send Notification failed!");
				_logger.LogError("Erro message: {0}", ex.Message);
			}
		}

		internal async Task StartProcess(HtmlDocument htmlDoc, List<Dictionary<string, string>> records,
										string chat_id, string token) {
			#region useful variables
			var pushList = new List<string>();
			var recordList = new List<Dictionary<string, string>>();
			#endregion

			#region data processing
			var apps = htmlDoc.DocumentNode.CssSelect("table tr.app"); //find all free games
			foreach (var each in apps) {
				if (each.Attributes.HasKeyIgnoreCase("hidden")) //skip the hidden trap row
					continue;

				var tds = each.CssSelect("td").ToArray();
				var tdLen = tds.Length; //steamDB added an extra column with a intall button

				//start gather free game basic info
				string subID = tds[1].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('/')[2];
				string gameName = tds[1].SelectSingleNode(".//b").InnerText;
				string gameURL = tds[0].SelectSingleNode(".//a[@href]").Attributes["href"].Value.Split('?')[0];
				string freeType = tdLen == 5 ? tds[2].InnerHtml.ToString() : tds[3].InnerHtml.ToString(); //steamDB added an extra column with a intall button
				DateTime startTime = DateTime.ParseExact(tdLen == 5 ? tds[3].Attributes["title"].Value.ToString() : tds[4].Attributes["title"].Value.ToString(), "yyyy-MM-dTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture).AddHours(8); //steamDB added an extra column with a intall button
				DateTime endTime = DateTime.ParseExact(tdLen == 5 ? tds[4].Attributes["title"].Value.ToString() : tds[5].Attributes["title"].Value.ToString(), "yyyy-MM-dTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture).AddHours(8); //steamDB added an extra column with a intall button

				if (freeType != "Weekend") {
					_logger.LogInformation("Found free game: {0}", gameName);
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
						if (steamName.Length > 0)
							gameName = steamName[0].InnerText;

						string pushMessage = "<b>" + gameName + "</b> \n\n";
						pushMessage += "Sub ID: <i>" + subID + "</i> \n";
						pushMessage += "链接: <a href=\"" + gameURL + "\" >" + gameName + "</a>\n";
						pushMessage += "开始时间: " + startTime.ToString() + "\n";
						pushMessage += "结束时间: " + endTime.ToString() + "\n";

						pushList.Add(pushMessage);
						_logger.LogInformation("Added game {0} in push list", gameName);
					}
				}
			}
			#endregion

			_logger.LogInformation("Writing records...");
			#region write records
			//write new record
			using (var jsonOp = new JsonOP()) {
				if (recordList.Count > 0) {
					jsonOp.WriteData(recordList, recordPath);
					_logger.LogDebug("Done writing records !");
				} else
					_logger.LogDebug("No records detected, quit writing records...");
			}
			#endregion

			_logger.LogInformation("Sending notification...");
			#region send notifications
			await SendNotification(id: chat_id, token: token, msgs: pushList);
			#endregion
		}

		internal async Task Run() {
			_logger.LogInformation(" - Start Job -");
			#region previous records and config file
			var records = new List<Dictionary<string, string>>();
			var config = new Dictionary<string, string>();
			#endregion

			using (var jsonOp = new JsonOP()) {

				_logger.LogInformation("Loading previous records...");
				#region load previous records
				try {
					records = jsonOp.LoadData(recordPath);// load previous free game info
				} catch (Exception e) {
					_logger.LogError("Error loading previous records !");
					_logger.LogError("Error message: { 0}\n", e.Message);
				}
				#endregion
				_logger.LogInformation("Done");

				_logger.LogInformation("Loading configurations...");
				#region load config
				try {
					config = jsonOp.LoadConfig(path: configPath);
				} catch (Exception e) {
					_logger.LogError("Error loading config file !");
					_logger.LogError("Error message: {0}", e.Message);
				}
				#endregion
				_logger.LogInformation("Done");
			}

			_logger.LogInformation("Getting page source...");
			var htmlDoc = new HtmlDocument();
			#region playright varialbles
			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Firefox.LaunchAsync(headless: true);
			#endregion

			#region load page
			try {
				var page = await browser.NewPageAsync();
				await page.GoToAsync(SteamDBUrl);
				Thread.Sleep(firstDelay);
				htmlDoc.LoadHtml(await page.GetInnerHtmlAsync("*"));
				_logger.LogInformation("Done");
			} catch (Exception ex) {
				_logger.LogError("Get source error!");
				_logger.LogError("Error message: {0}", ex.Message);
			} finally {
				await browser.CloseAsync();
			}
			#endregion

			_logger.LogInformation("Start data processing...");
			await StartProcess(htmlDoc: htmlDoc, records: records, chat_id: config["CHAT_ID"], token: config["TOKEN"]);
			_logger.LogInformation("Done");

			_logger.LogInformation(" - End Job -");
		}

		static async Task Main() {
			#region config service
			var services = new ServiceCollection();
			ConfigureServices(services);
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			Program app = serviceProvider.GetService<Program>();
			await app.Run();
			#endregion
		}
	}
}
