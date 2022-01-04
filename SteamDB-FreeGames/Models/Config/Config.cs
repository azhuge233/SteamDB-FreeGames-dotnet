namespace SteamDB_FreeGames.Models {
	public class Config: ASFConfig {
		public bool EnableHeadless { get; set; }
		public bool NotifyKeepGamesOnly { get; set; }
		public int TimeOutMilliSecond { get; set; }
	}
}
