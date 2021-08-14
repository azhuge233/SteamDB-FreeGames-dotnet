﻿using System;
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
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			if (records.Count == 0) {
				_logger.LogInformation($"{debugSendMessage} : No new notifications !");
				return;
			}

			try {
				var sb = new StringBuilder();
				string url = new StringBuilder().AppendFormat(PushMessageFormat.barkUrlFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();

				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Name}");
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
					await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(PushMessageFormat.barkUrlTitle)
							.Append(HttpUtility.UrlEncode(record.ToBarkMessage()))
							.Append(new StringBuilder().AppendFormat(PushMessageFormat.barkUrlArgs, record.ID))
							.ToString()
					);
				}

				await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(PushMessageFormat.barkUrlTitle)
							.Append(HttpUtility.UrlEncode(sb.ToString()))
							.Append(new StringBuilder().AppendFormat(PushMessageFormat.barkUrlArgs, sb.ToString()))
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
