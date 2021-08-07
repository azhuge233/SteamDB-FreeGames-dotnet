using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	class PlayWrightOP : IDisposable {
		private readonly ILogger<PlayWrightOP> _logger;
		private readonly string SteamDBUrl = "https://steamdb.info/upcoming/free/";
		private readonly int firstDelay = 10000;

		private readonly string debugPlaywright = "Get page source";

		public PlayWrightOP(ILogger<PlayWrightOP> logger) {
			_logger = logger;
		}

		public async Task<string> GetHtmlSource(bool useHeadless = true) {
			try {
				_logger.LogDebug(debugPlaywright);
				Microsoft.Playwright.Program.Main(new string[] { "install", "webkit" }); // From https://github.com/microsoft/playwright-dotnet/issues/1545#issuecomment-865199736
				string source;
				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Webkit.LaunchAsync(new() { Headless = useHeadless });

				var page = await browser.NewPageAsync();
				await page.GotoAsync(SteamDBUrl);
				Thread.Sleep(firstDelay);
				source = await page.InnerHTMLAsync("*");
				_logger.LogDebug($"Done: {debugPlaywright}");
				return source;
			} catch (Exception) {
				_logger.LogError($"Error: {debugPlaywright}");
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
