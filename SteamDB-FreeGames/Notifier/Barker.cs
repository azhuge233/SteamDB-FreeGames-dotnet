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
		private readonly string debugSendMessage = "Send notification to Bark";
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(string address, string token, List<FreeGameRecord> records) {
			if (records.Count == 0) {
				_logger.LogInformation($"{debugSendMessage} : No new notifications !");
				return;
			}

			try {
				_logger.LogDebug(debugSendMessage);

				var sb = new StringBuilder();
				string url = new StringBuilder().AppendFormat(barkUrlFormat, address, token).ToString();
				var webGet = new HtmlWeb();

				foreach (var record in records) {
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
					await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(barkUrlTitle)
							.Append(HttpUtility.UrlEncode(record.ToBarkMessage()))
							.Append(new StringBuilder().AppendFormat(barkUrlArgs, record.ID))
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

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessage}");
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
