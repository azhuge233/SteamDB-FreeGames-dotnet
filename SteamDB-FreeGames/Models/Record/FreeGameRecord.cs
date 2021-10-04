using System;

namespace SteamDB_FreeGames.Models {
	public class FreeGameRecord {
		public string ID { get; set; }
		public string Url { get; set; }
	
		public string Name { get; set; }

		public string FreeType { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }
	}
}
