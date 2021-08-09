using System;
using System.Collections.Generic;
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

		public void CheckValid(Dictionary<string, string> config) {
			try {
				_logger.LogDebug(debugCheckValid);

				//Telegram
				if (Convert.ToBoolean(config[ConfigKeys.EnableTelegramKey])) {
					if (config[ConfigKeys.TelegramTokenKey] == string.Empty)
						throw new Exception(message: "No Telegram Token provided!");
					if (config[ConfigKeys.TelegramChatIDKey] == string.Empty)
						throw new Exception(message: "No Telegram ChatID provided!");
				}

				//Bark
				if (Convert.ToBoolean(config[ConfigKeys.EnableBarkKey])) {
					if (config[ConfigKeys.BarkAddressKey] == string.Empty)
						throw new Exception(message: "No Bark Address provided!");
					if (config[ConfigKeys.BarkTokenKey] == string.Empty)
						throw new Exception(message: "No Bark Token provided!");
				}

				//Other configs
				if (config[ConfigKeys.KeepGamesOnlyKey] == string.Empty)
					throw new Exception(message: $"{ConfigKeys.KeepGamesOnlyKey} not configured!");
				if (config[ConfigKeys.TimeOutSecKey] == string.Empty)
					throw new Exception(message: $"{ConfigKeys.TimeOutSecKey} not configured!");
				if (config[ConfigKeys.UseHeadlessKey] == string.Empty)
					throw new Exception(message: $"{ConfigKeys.UseHeadlessKey} not configured!");

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
