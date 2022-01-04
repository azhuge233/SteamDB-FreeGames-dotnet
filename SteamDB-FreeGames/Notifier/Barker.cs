using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class Barker: INotifiable {
		private readonly ILogger<Barker> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Bark";
		private readonly string debugSendMessageASF = "Send ASF result to Bark";
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<NotifyRecord> records) {
			try {
				var sb = new StringBuilder();
				string url = new StringBuilder().AppendFormat(NotifyFormatStrings.barkUrlFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Name}");
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
					resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(NotifyFormatStrings.barkUrlTitle)
							.Append(HttpUtility.UrlEncode(record.ToBarkMessage(update: record.IsUpdate)))
							.Append(HttpUtility.UrlEncode(NotifyFormatStrings.projectLink))
							.Append(new StringBuilder().AppendFormat(NotifyFormatStrings.barkUrlArgs, record.ID))
							.ToString()
					);
					_logger.LogDebug(resp.Text);
				}

				resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(NotifyFormatStrings.barkUrlTitle)
							.Append(HttpUtility.UrlEncode(sb.ToString()))
							.Append(new StringBuilder().AppendFormat(NotifyFormatStrings.barkUrlArgs, sb.ToString()))
							.ToString()
					);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task SendMessage(NotifyConfig config, string asfRecord) {
			try {
				_logger.LogDebug(debugSendMessageASF);
				string url = new StringBuilder().AppendFormat(NotifyFormatStrings.barkUrlFormat, config.BarkAddress, config.BarkToken).ToString();

				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				resp = await webGet.LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
						.Append(NotifyFormatStrings.barkUrlASFTitle)
						.Append(HttpUtility.UrlEncode(asfRecord))
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
