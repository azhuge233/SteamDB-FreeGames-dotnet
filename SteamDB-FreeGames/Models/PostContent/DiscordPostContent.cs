using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamDB_FreeGames.Models.PostContent {
	public class Footer {
		[JsonProperty("text")]
		public string Text { get; set; }

	}
	public class Embed {
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("color")]
		public int Color { get; set; } = 2721196;
		[JsonProperty("footer")]
		public Footer Footer { get; set; }
	}
	public class DiscordPostContent {
		[JsonProperty("content")]
		public string Content { get; set; }
		[JsonProperty("embeds")]
		public List<Embed> Embeds { get; set; } = new List<Embed>();
	}
}
