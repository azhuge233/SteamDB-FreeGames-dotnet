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

		public PlayWrightOP(ILogger<PlayWrightOP> logger) {
			_logger = logger;
		}

		public async Task<string> GetHtmlSource() {
			try {
				Microsoft.Playwright.Program.Main(new string[] { "install", "webkit" }); // From https://github.com/microsoft/playwright-dotnet/issues/1545#issuecomment-865199736
				_logger.LogDebug("Getting page source");
				string source;
				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Webkit.LaunchAsync(new() { Headless = true });

				var page = await browser.NewPageAsync();
				await page.GotoAsync(SteamDBUrl);
				Thread.Sleep(firstDelay);
				source = await page.InnerHTMLAsync("*");
				_logger.LogDebug("Done");
				return source;
			} catch (Exception) {
				_logger.LogError("Getting page source Error");
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
