using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	internal class PushDeer: INotifiable {
		private readonly ILogger<PushDeer> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushDeer";
		private readonly string debugSendMessageASF = "Send ASF result to PushDeer";
		#endregion

		public PushDeer(ILogger<PushDeer> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<NotifyRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);
				var sb = new StringBuilder();
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Name}");
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
					resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
						.AppendFormat(NotifyFormatStrings.pushDeerUrlFormat,
									config.PushDeerToken,
									HttpUtility.UrlEncode(record.ToPushDeerMessage(update: record.IsUpdate)))
						.Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
						.ToString()
					);
					_logger.LogDebug(resp.Text);
				}

				resp = await webGet.LoadFromWebAsync(
					new StringBuilder()
					.AppendFormat(NotifyFormatStrings.pushDeerUrlFormat,
								config.PushDeerToken,
								HttpUtility.UrlEncode(sb.ToString()))
					.ToString()
				);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task SendMessage(NotifyConfig config, string asfRecord) {
			try {
				_logger.LogDebug(debugSendMessageASF);

				var resp = await new HtmlWeb().LoadFromWebAsync(
					new StringBuilder()
					.AppendFormat(NotifyFormatStrings.pushDeerUrlFormat,
								config.PushDeerToken,
								HttpUtility.UrlEncode(asfRecord))
					.ToString()
				);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {debugSendMessageASF}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessageASF}");
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
