using System;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames {
	class ConfigValidator: IDisposable {
		private readonly ILogger<ConfigValidator> _logger;

		#region debug strings
		private readonly string debugCheckValid = "Check config file validation";
		#endregion

		public ConfigValidator(ILogger<ConfigValidator> logger) {
			_logger = logger;
		}

		public void CheckValid(Config config) {
			try {
				_logger.LogDebug(debugCheckValid);

				//Telegram
				if (config.EnableTelegram) {
					if (config.TelegramToken == string.Empty)
						throw new Exception(message: "No Telegram Token provided!");
					if (config.TelegramChatID == string.Empty)
						throw new Exception(message: "No Telegram ChatID provided!");
				}

				//Bark
				if (config.EnableBark) {
					if (config.BarkAddress == string.Empty)
						throw new Exception(message: "No Bark Address provided!");
					if (config.BarkToken == string.Empty)
						throw new Exception(message: "No Bark Token provided!");
				}

				_logger.LogDebug($"Done: {debugCheckValid}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugCheckValid}");
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
