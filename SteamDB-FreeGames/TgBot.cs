﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	public class TgBot: IDisposable {
		private readonly ILogger<TgBot> _logger;
		private TelegramBotClient BotClient { get; set; }

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(string token, string chatID, List<string> msgs, bool htmlMode = false) {
			if (msgs.Count == 0) {
				_logger.LogInformation("No new notifications !");
				return;
			}

			BotClient = new TelegramBotClient(token: token);
			int count = 1;
			try {
				foreach (var msg in msgs) {
					_logger.LogDebug("Sending Message {0}", count++);
					await BotClient.SendTextMessageAsync(
						chatId: chatID,
						text: msg,
						parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
					);
				}
			} catch (Exception) {
				_logger.LogError("Send notification failed.");
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
