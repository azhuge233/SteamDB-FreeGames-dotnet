using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Modules {
	class Scraper : IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string SteamDBUrl = "https://steamdb.info/upcoming/free/";

		#region debug strings
		private readonly string debugGetSteamSource = "Get Steam page source";
		private readonly string debugGetSteamDBSource = "Get SteamDB page source";
		#endregion

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public HtmlDocument GetSteamSource(string url) {
			try {
				_logger.LogDebug(debugGetSteamSource);

				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(url);

				_logger.LogDebug($"Done: {debugGetSteamSource}");
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSteamSource}");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task<string> GetSteamDBSource(Config config) {
			try {
				_logger.LogDebug(debugGetSteamDBSource);

				Microsoft.Playwright.Program.Main(new string[] { "install", "webkit" }); // From https://github.com/microsoft/playwright-dotnet/issues/1545#issuecomment-865199736
				string source;
				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Webkit.LaunchAsync(new() { Headless = config.EnableHeadless });

				var page = await browser.NewPageAsync();
				page.SetDefaultTimeout(config.TimeOutMilliSecond);
				page.SetDefaultNavigationTimeout(config.TimeOutMilliSecond);

				await page.GotoAsync(SteamDBUrl);
				await page.WaitForSelectorAsync("div.body-content");
				await page.WaitForLoadStateAsync();
				source = await page.InnerHTMLAsync("*");

				_logger.LogDebug($"Done: {debugGetSteamDBSource}");
				return source;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetSteamDBSource}");
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
