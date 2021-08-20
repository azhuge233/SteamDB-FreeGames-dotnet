using System;
using System.Text;

namespace SteamDB_FreeGames.Models {
	public class FreeGameRecord {
		public string ID { get; set; }
		public string Url { get; set; }
	
		public string Name { get; set; }

		public string FreeType { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }

		public string ToTelegramMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.telegramPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToBarkMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.barkPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToEmailMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.emailPushHtmlFormat, Name, ID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToQQMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.qqPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToPushPlusMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusPushHtmlFormat, Name, ID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToDingTalkMessage() {
			return new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}
	}
}
