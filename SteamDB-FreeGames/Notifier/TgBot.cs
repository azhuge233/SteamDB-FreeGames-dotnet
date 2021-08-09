using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames {
	public class TgBot: IDisposable {
		private readonly ILogger<TgBot> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Telegram";
		#endregion

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(string token, string chatID, List<FreeGameRecord> records, bool htmlMode = false) {
			if (records.Count == 0) {
				_logger.LogInformation($"{debugSendMessage} : No new notifications !");
				return;
			}

			var sb = new StringBuilder();
			var BotClient = new TelegramBotClient(token: token);

			try {
				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Name}");
					await BotClient.SendTextMessageAsync(
						chatId: chatID,
						text: record.ToTelegramMessage(),
						parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
					);
					sb.Append($"{record.SubID} ");
				}

				await BotClient.SendTextMessageAsync(
						chatId: chatID,
						text: sb.ToString(),
						parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
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
