﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;
using SteamDB_FreeGames.Notifier;

namespace SteamDB_FreeGames {
	public class TgBot: INotifiable {
		private readonly ILogger<TgBot> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Telegram";
		#endregion

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			if (records.Count == 0) {
				_logger.LogInformation($"{debugSendMessage} : No new notifications !");
				return;
			}

			var sb = new StringBuilder();
			var BotClient = new TelegramBotClient(token: config.TelegramToken);

			try {
				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Name}");
					await BotClient.SendTextMessageAsync(
						chatId: config.TelegramChatID,
						text: record.ToTelegramMessage(), 
						parseMode: ParseMode.Html
					);
					sb.Append(sb.Length == 0 ? record.ID : $",{record.ID}");
				}

				await BotClient.SendTextMessageAsync(
						chatId: config.TelegramChatID,
						text: sb.ToString(),
						parseMode: ParseMode.Html
				);

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
