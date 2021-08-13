namespace SteamDB_FreeGames.Models {
	public class Config {
		public bool EnableHeadless { get; set; }
		public bool NotifyKeepGamesOnly { get; set; }
		public int TimeOutMilliSecond { get; set; }

		public bool EnableTelegram { get; set; }
		public string TelegramToken { get; set; }
		public string TelegramChatID { get; set; }

		public bool EnableBark { get; set; }
		public string BarkAddress { get; set; }
		public string BarkToken { get; set; }

		public bool EnableEmail { get; set; }
		public string SMTPServer { get; set; }
		public int SMTPPort { get; set; }
		public string FromEmailAddress { get; set; }
		public string ToEmailAddress { get; set; }
		public string AuthAccount { get; set; }
		public string AuthPassword { get; set; }
	}
}
