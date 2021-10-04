namespace SteamDB_FreeGames.Models {
	public static class NotifyFormatStrings {
		#region record ToMessage(update = false) string
		public static readonly string telegramPushFormat =
			"<b>{0}</b>\n\n" +
			"Sub/App ID: <i>{1}</i>\n" +
			"免费类型: {2}\n" +
			"链接: <a href=\"{3}\" > {0}</a>\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}\n\n" + 
			"#Steam #{2} #{6}";
		public static readonly string barkPushFormat =
			"{0}\n" +
			"Sub/App ID: {1}\n" +
			"免费类型: {2}\n" +
			"链接: {3}\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}";
		public static readonly string emailPushHtmlFormat =
			"<p><b>{0}</b><br>" +
			"Sub/App ID: {1}<br>" +
			"免费类型: {2}<br>" +
			"链接: <a href=\"{3}\" > {0}</a><br>" +
			"开始时间: {4}<br>" +
			"结束时间: {5}</p>";
		public static readonly string qqPushFormat =
			"{0}\n" +
			"Sub/App ID: {1}\n" +
			"免费类型: {2}\n" +
			"链接: {3}\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}";
		public static readonly string pushPlusPushHtmlFormat =
			"<p><b>{0}</b><br>" +
			"Sub/App ID: {1}<br>" +
			"免费类型: {2}<br>" +
			"链接: <a href=\"{3}\" > {0}</a><br>" +
			"开始时间: {4}<br>" +
			"结束时间: {5}</p>";
		public static readonly string dingTalkPushFormat =
			"{0}\n" +
			"Sub/App ID: {1}\n" +
			"免费类型: {2}\n" +
			"链接: {3}\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}";
		#endregion

		#region record ToMessage(update = true) string
		public static readonly string telegramUpdatePushFormat =
			"<b>{0}</b> <i>信息更新</i>\n\n" +
			"Sub/App ID: <i>{1}</i>\n" +
			"免费类型: {2}\n" +
			"链接: <a href=\"{3}\" > {0}</a>\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}\n\n" +
			"#Steam #{2} #{6} #Update";
		public static readonly string barkUpdatePushFormat =
			"{0} 信息更新\n" +
			"Sub/App ID: {1}\n" +
			"免费类型: {2}\n" +
			"链接: {3}\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}";
		public static readonly string emailUpdatePushHtmlFormat =
			"<p><b>{0}</b> <i>信息更新</i><br>" +
			"Sub/App ID: {1}<br>" +
			"免费类型: {2}<br>" +
			"链接: <a href=\"{3}\" > {0}</a><br>" +
			"开始时间: {4}<br>" +
			"结束时间: {5}</p>";
		public static readonly string qqUpdatePushFormat =
			"{0} 信息更新\n" +
			"Sub/App ID: {1}\n" +
			"免费类型: {2}\n" +
			"链接: {3}\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}";
		public static readonly string pushPlusUpdatePushHtmlFormat =
			"<p><b>{0}</b> <i>信息更新</i><br>" +
			"Sub/App ID: {1}<br>" +
			"免费类型: {2}<br>" +
			"链接: <a href=\"{3}\" > {0}</a><br>" +
			"开始时间: {4}<br>" +
			"结束时间: {5}</p>";
		public static readonly string dingTalkUpdatePushFormat =
			"{0} 信息更新\n" +
			"Sub/App ID: {1}\n" +
			"免费类型: {2}\n" +
			"链接: {3}\n" +
			"开始时间: {4}\n" +
			"结束时间: {5}";
		#endregion

		#region url, title format string
		public static readonly string barkUrlFormat = "{0}/{1}/";
		public static readonly string barkUrlTitle = "SteamDB-FreeGames/";
		public static readonly string barkUrlArgs =
			"?group=steamdbfreegames" +
			"&copy={0}" +
			"&isArchive=1" +
			"&sound=calypso";

		public static readonly string emailTitleFormat = "{0} new free game(s) - SteamDB-FreeGames";
		public static readonly string emailBodyFormat = "<br>{0}";

		public static readonly string qqUrlFormat = "http://{0}:{1}/send_private_msg?user_id={2}&message=";

		public static readonly string pushPlusTitleFormat = "{0} new free game(s) - SteamDB-FreeGames";
		public static readonly string pushPlusBodyFormat = "<br>{0}";
		public static readonly string pushPlusUrlFormat = "http://www.pushplus.plus/send?token={0}&template=html&title={1}&content=";

		public static readonly string dingTalkUrlFormat = "https://oapi.dingtalk.com/robot/send?access_token={0}";
		#endregion


		public static readonly string projectLink = "\n\nFrom https://github.com/azhuge233/SteamDB-FreeGames-dotnet";
		public static readonly string projectLinkHTML = "<br><br>From <a href=\"https://github.com/azhuge233/SteamDB-FreeGames-dotnet\">SteamDB-FreeGames</a>";
	}
}
