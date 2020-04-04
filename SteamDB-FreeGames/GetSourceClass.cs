using System;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace SteamDB_FreeGames {
	public class GetSourceClass: IDisposable {

		public string getSource(string url) {
			var chromeOptions = new ChromeOptions();
			chromeOptions.AddArgument("--no-sandbox");
			chromeOptions.AddArgument("--disable-dev-shm-usage");
			chromeOptions.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);

			var mychrome = new ChromeDriver(chromeOptions);
			mychrome.Navigate().GoToUrl(url);
			Thread.Sleep(8000);
			var source = mychrome.PageSource;
			mychrome.Quit();

			return source;
		}

		public void Dispose() { }
	}
}
