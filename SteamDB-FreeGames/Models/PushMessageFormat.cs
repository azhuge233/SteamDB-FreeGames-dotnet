namespace SteamDB_FreeGames.Models {
	public class PushMessageFormat {
		public static readonly string telegramPushFormat =
			"<b>{0}</b>\n\n" +
			"Sub/App ID: <i>{1}</i>\n" +
			"免费类型: {2}\n" +
			"链接: <a href=\"{3}\" > {4}</a>\n" +
			"开始时间: {5}\n" +
			"结束时间: {6}\n";

		public static readonly string barkPushFormat =
			"{0}\nSub/App ID: {1}\n免费类型: {2}\n" +
			"链接: {3}\n开始时间: {4}\n结束时间: {5}";

		public static readonly string emailPushHtmlFormat =
			"<p><b>{0}</b><br>" +
			"Sub/App ID: {1}<br>" +
			"免费类型: {2}<br>" +
			"链接: <a href=\"{3}\" > {4}</a><br>" +
			"开始时间: {5}<br>" +
			"结束时间: {6}</p>";
	}
}
