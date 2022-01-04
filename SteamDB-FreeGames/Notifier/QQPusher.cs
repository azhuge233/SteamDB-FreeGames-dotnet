using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class QQPusher: INotifiable {
		private readonly ILogger<QQPusher> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to QQ";
		private readonly string debugSendMessageASF = "Send ASF result to QQ";
		#endregion

		public QQPusher(ILogger<QQPusher> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<NotifyRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				string url = new StringBuilder().AppendFormat(NotifyFormatStrings.qqUrlFormat, config.QQAddress, config.QQPort, config.ToQQID).ToString();
				var sb = new StringBuilder();
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Name}");
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
					resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(HttpUtility.UrlEncode(record.ToQQMessage(update: record.IsUpdate)))
							.Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
							.ToString()
					);
					_logger.LogDebug(resp.Text);
				}

				resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(HttpUtility.UrlEncode(sb.ToString()))
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

		public async Task SendMessage(NotifyConfig config, string asfResult) {
			try {
				_logger.LogDebug(debugSendMessageASF);

				string url = new StringBuilder().AppendFormat(NotifyFormatStrings.qqUrlFormat, config.QQAddress, config.QQPort, config.ToQQID).ToString();
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(HttpUtility.UrlEncode(asfResult))
							.ToString()
					);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {debugSendMessageASF}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
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
