using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	class Scraper : IDisposable {
		private readonly ILogger<Scraper> _logger;
		private readonly string SteamDBUrl = "https://steamdb.info/upcoming/free/";
		//private readonly int firstDelay = 10000;
		private readonly int timeOut = 30000;

		private readonly string debugGetSteamSource = "Get Steam page source";
		private readonly string debugGetSteamDBSource = "Get SteamDB page source";

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

		public async Task<string> GetSteamDBSource(bool useHeadless = true) {
			try {
				_logger.LogDebug(debugGetSteamDBSource);

				Microsoft.Playwright.Program.Main(new string[] { "install", "webkit" }); // From https://github.com/microsoft/playwright-dotnet/issues/1545#issuecomment-865199736
				string source;
				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Webkit.LaunchAsync(new() { Headless = useHeadless });

				var page = await browser.NewPageAsync();
				page.SetDefaultTimeout(timeOut);
				page.SetDefaultNavigationTimeout(timeOut);

				await page.GotoAsync(SteamDBUrl);
				await page.WaitForSelectorAsync("div.body-content");
				await page.WaitForLoadStateAsync();
				//Thread.Sleep(firstDelay);
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
