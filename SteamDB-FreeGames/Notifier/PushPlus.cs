﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class PushPlus: INotifiable {
		private readonly ILogger<PushPlus> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to PushPlus";
		private readonly string debugCreateMessage = "Create notification message";
		private readonly string debugSendMessageASF = "Send ASF result to PushPlus";
		#endregion

		public PushPlus(ILogger<PushPlus> logger) {
			_logger = logger;
		}

		private string CreateMessage(List<NotifyRecord> records) {
			try {
				_logger.LogDebug(debugCreateMessage);

				var sb = new StringBuilder();
				var sbSubID = new StringBuilder();

				records.ForEach(record => {
					sbSubID.Append(sbSubID.Length == 0 ? record.ID : $",{record.ID}");
					sb.AppendFormat(NotifyFormatStrings.pushPlusBodyFormat, record.ToPushPlusMessage(update: record.IsUpdate));
				});

				sbSubID.Append("<br>").Append(sb).Append(NotifyFormatStrings.projectLinkHTML);

				_logger.LogDebug($"Done: {debugCreateMessage}");
				return HttpUtility.UrlEncode(sbSubID.ToString());
			} catch (Exception) {
				_logger.LogError($"Error: {debugCreateMessage}");
				throw;
			}
		}

		public async Task SendMessage(NotifyConfig config, List<NotifyRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var title = HttpUtility.UrlEncode(new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusTitleFormat, records.Count).ToString());
				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusUrlFormat, config.PushPlusToken, title);
				var message = CreateMessage(records);

				var resp = await new HtmlWeb().LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
						.Append(message)
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

				var title = HttpUtility.UrlEncode(NotifyFormatStrings.pushPlusASFTitleFormat);
				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusUrlFormat, config.PushPlusToken, title);

				var resp = await new HtmlWeb().LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
						.Append(HttpUtility.UrlEncode(asfResult.Replace("<", "&lt;").Replace(">", "&gt;")))
						.ToString()
				);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {debugSendMessageASF}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessageASF}");
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
