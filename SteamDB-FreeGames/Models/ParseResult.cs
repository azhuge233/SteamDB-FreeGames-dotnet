using System.Collections.Generic;

namespace SteamDB_FreeGames.Models {
	public class ParseResult {
		public List<FreeGameRecord> PushListAll { get; set; }

		public List<FreeGameRecord> PushListKeepOnly { get; set; }

		public List<FreeGameRecord> Records { get; set; }

		public ParseResult() {
			PushListAll = new List<FreeGameRecord>();
			PushListKeepOnly = new List<FreeGameRecord>();
			Records = new List<FreeGameRecord>();
		}
	}
}
