namespace SteamDB_FreeGames.Models {
	public class Config {
		public bool EnableHeadless { get; set; }
		public bool KeepGamesOnly { get; set; }
		public int TimeOutMilliSecond { get; set; }

		public bool EnableTelegram { get; set; }
		public string TelegramToken { get; set; }
		public string TelegramChatID { get; set; }

		public bool EnableBark { get; set; }
		public string BarkAddress { get; set; }
		public string BarkToken { get; set; }

	}
}
