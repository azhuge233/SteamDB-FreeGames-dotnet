using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class DingTalk: INotifiable {
		private readonly ILogger<DingTalk> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notifications to DingTalk";
		#endregion

		public DingTalk(ILogger<DingTalk> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<NotifyRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkUrlFormat, config.DingTalkBotToken).ToString();
				var content = new DingTalkPostContent();

				var sb = new StringBuilder();
				var client = new HttpClient();
				var data = new StringContent("");
				var resp = new HttpResponseMessage();

				foreach (var record in records) {
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
					content.text.content = $"{record.ToDingTalkMessage(update: record.IsUpdate)}{NotifyFormatStrings.projectLink}";
					data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					resp = await client.PostAsync(url, data);
					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
				}

				content.text.content = sb.ToString();
				data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				resp = await client.PostAsync(url, data);
				_logger.LogDebug(await resp.Content.ReadAsStringAsync());

				_logger.LogDebug($"Done: {debugSendMessage}");
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
