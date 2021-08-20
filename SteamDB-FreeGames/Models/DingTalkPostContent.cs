namespace SteamDB_FreeGames.Models {
	public class Content {
#pragma warning disable IDE1006
		public string content { get; set; }
#pragma warning restore IDE1006
	}
	public class DingTalkPostContent {
#pragma warning disable IDE1006 // 命名样式
		public string msgtype { get; set; }
		public Content text { get; set; }
#pragma warning restore IDE1006

		public DingTalkPostContent() {
			msgtype = "text";
			text = new Content();
		}
	}
}
