namespace SteamDB_FreeGames.Models {
	public static class ParseStrings {
		#region XPath strings
		public static readonly string XPathRecord = ".//div[@class=\'container\']//table//tr[@class=\'app\']";
		public static readonly string XPathtds = ".//td";
		#endregion

		public static readonly string SteamDBDateFormat = "yyyy-MM-dTHH:mm:ss+00:00";
		public static readonly string keepGameString = "Keep";
		public static readonly string hiddenAttribute = "hidden";
	}
}
