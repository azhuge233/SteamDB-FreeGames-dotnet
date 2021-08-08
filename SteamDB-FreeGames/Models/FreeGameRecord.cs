using System;
using System.Text;

namespace SteamDB_FreeGames.Models {
	public class FreeGameRecord {
		private readonly string pushFormat = "<b>{0}</b>\n\n" +
			"Sub ID: <i>{1}</i>\n" +
			"免费类型: {2}\n" +
			"链接: <a href=\"{3}\" > {4}</a>\n" +
			"开始时间: {5}\n" +
			"结束时间: {6}\n";

		public string SubID { get; set; }
		public string Url { get; set; }
	
		public string Name { get; set; }

		public string FreeType { get; set; }
		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string ToMessage() {
			return new StringBuilder().AppendFormat(pushFormat, Name, SubID, FreeType, Url, Name, StartTime.ToString(), EndTime.ToString()).ToString();
		}
	}
}
