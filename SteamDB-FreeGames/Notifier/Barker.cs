using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class Barker: IDisposable {
		private readonly ILogger<Barker> _logger;

		#region push message format strings
		private readonly string barkUrlFormat = "{0}/{1}/";
		private readonly string barkUrlTitle = "SteamDB-FreeGames/";
		private readonly string barkUrlArgs = 
			"?group=steamdbfreegames" +
			"&copy={0}" +
			"&isArchive=1" +
			"&sound=calypso";
		#endregion

		#region debug strings
		private readonly string debugBark = "Send notification to Bark";
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(string address, string token, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugBark);

				var sb = new StringBuilder();
				string url = new StringBuilder().AppendFormat(barkUrlFormat, address, token).ToString();
				var webGet = new HtmlWeb();

				foreach (var record in records) {
					sb.Append($"{record.SubID} ");
					await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(barkUrlTitle)
							.Append(HttpUtility.UrlEncode(record.ToBarkMessage()))
							.Append(new StringBuilder().AppendFormat(barkUrlArgs, record.SubID))
							.ToString()
					);
				}

				await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(barkUrlTitle)
							.Append(HttpUtility.UrlEncode(sb.ToString()))
							.Append(new StringBuilder().AppendFormat(barkUrlArgs, sb.ToString()))
							.ToString()
					);

				_logger.LogDebug($"Done: {debugBark}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugBark}");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
