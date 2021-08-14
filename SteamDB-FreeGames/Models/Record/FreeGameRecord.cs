using System;
using System.Text;

namespace SteamDB_FreeGames.Models {
	public class FreeGameRecord {
		public string ID { get; set; }
		public string Url { get; set; }
	
		public string Name { get; set; }

		public string FreeType { get; set; }
		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string ToTelegramMessage() {
			return new StringBuilder().AppendFormat(PushMessageFormat.telegramPushFormat, Name, ID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToBarkMessage() {
			return new StringBuilder().AppendFormat(PushMessageFormat.barkPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString(), ID).ToString();
		}

		public string ToEmailMessage() {
			return new StringBuilder().AppendFormat(PushMessageFormat.emailPushHtmlFormat, Name, ID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}
	}
}
