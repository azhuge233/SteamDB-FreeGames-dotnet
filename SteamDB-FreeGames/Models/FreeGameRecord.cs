using System;
using System.Text;

namespace SteamDB_FreeGames.Models {
	public class FreeGameRecord {
		#region push message format strings
		private readonly string telegramPushFormat = 
			"<b>{0}</b>\n\n" +
			"Sub/App ID: <i>{1}</i>\n" +
			"免费类型: {2}\n" +
			"链接: <a href=\"{3}\" > {4}</a>\n" +
			"开始时间: {5}\n" +
			"结束时间: {6}\n";

		private readonly string barkPushFormat =
			"{0}\nSub/App ID: {1}\n免费类型: {2}\n" +
			"链接: {3}\n开始时间: {4}\n结束时间: {5}";

		private readonly string emailPushHtmlFormat =
			"<p><b>{0}</b><br>" +
			"Sub/App ID: {1}<br>" +
			"免费类型: {2}<br>" +
			"链接: <a href=\"{3}\" > {4}</a><br>" +
			"开始时间: {5}<br>" +
			"结束时间: {6}</p>";
		#endregion

		public string ID { get; set; }
		public string Url { get; set; }
	
		public string Name { get; set; }

		public string FreeType { get; set; }
		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string ToTelegramMessage() {
			return new StringBuilder().AppendFormat(telegramPushFormat, Name, ID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToBarkMessage() {
			return new StringBuilder().AppendFormat(barkPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString(), ID).ToString();
		}

		public string ToEmailMessage() {
			return new StringBuilder().AppendFormat(emailPushHtmlFormat, Name, ID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}
	}
}
