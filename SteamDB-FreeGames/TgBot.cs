using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SteamDB_FreeGames {
	public class TgBot: IDisposable {
		private TelegramBotClient BotClient;

		public TgBot(string token) {
			this.BotClient = new TelegramBotClient(token: token);
		}

		public async Task SendMessage(string chatId, string msg, bool htmlMode = false) {
			if (msg != string.Empty) {
				await BotClient.SendTextMessageAsync(
					chatId: chatId,
					text: msg,
					parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
				);
			}
		}

		public void Dispose() { }
	}
}
