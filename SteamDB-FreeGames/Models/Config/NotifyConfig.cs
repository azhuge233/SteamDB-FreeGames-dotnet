namespace SteamDB_FreeGames.Models {
	public class NotifyConfig {
		public bool EnableTelegram { get; set; }
		public bool EnableBark { get; set; }
		public bool EnableEmail { get; set; }

		public string TelegramToken { get; set; }
		public string TelegramChatID { get; set; }

		public string BarkAddress { get; set; }
		public string BarkToken { get; set; }

		public string SMTPServer { get; set; }
		public int SMTPPort { get; set; }
		public string FromEmailAddress { get; set; }
		public string ToEmailAddress { get; set; }
		public string AuthAccount { get; set; }
		public string AuthPassword { get; set; }

		public bool EnableQQ { get; set; }
		public string QQAddress { get; set; }
		public string QQPort { get; set; }
		public string ToQQID { get; set; }

		public bool EnablePushPlus { get; set; }
		public string PushPlusToken { get; set; }

		public bool EnableDingTalk { get; set; }
		public string DingTalkBotToken { get; set; }

		public bool EnablePushDeer { get; set; }
		public string PushDeerToken { get; set; }

		public bool EnableDiscord { get; set; }
		public string DiscordWebhookURL { get; set; }
	}
}
