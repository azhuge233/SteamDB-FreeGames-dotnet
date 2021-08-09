namespace SteamDB_FreeGames.Models {
	public static class ConfigKeys {
		public static string UseHeadlessKey { get; private set; } = "ENABLE_HEADLESS";
		public static string KeepGamesOnlyKey { get; private set; } = "KEEP_GAMES_ONLY";
		public static string TimeOutSecKey { get; private set; } = "TIMEOUT_SEC";
		public static string EnableTelegramKey { get; private set; } = "ENABLE_TELEGRAM";
		public static string TelegramTokenKey { get; private set; } = "TOKEN";
		public static string TelegramChatIDKey { get; private set; } = "CHAT_ID";
		public static string EnableBarkKey { get; private set; } = "ENABLE_BARK";
		public static string BarkAddressKey { get; private set; } = "BARK_ADDR";
		public static string BarkTokenKey { get; private set; } = "BARK_TOKEN";
	}
}
