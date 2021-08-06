using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	public class TgBot: IDisposable {
		private readonly ILogger<TgBot> _logger;
		private TelegramBotClient BotClient { get; set; }
		private readonly string debugSendMessage = "Sending Message";

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
					_logger.LogDebug($"{debugSendMessage} {count++}");
					await BotClient.SendTextMessageAsync(
						chatId: chatID,
						text: msg,
						parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
					);
				}
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
