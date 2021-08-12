﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SteamDB_FreeGames.Notifier;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Modules {
	class NotifyOP:IDisposable {
		private readonly ILogger<NotifyOP> _logger;
		private readonly IServiceProvider services = DI.BuildDiNotifierOnly();

		#region debug strings
		private readonly string debugNotify = "Notify";
		private readonly string debugDisabledFormat = "{0} notify is disabled, skipping";
		#endregion


		public NotifyOP(ILogger<NotifyOP> logger) {
			_logger = logger;
		}

		public async Task Notify(Config config, List<FreeGameRecord> pushList) {
			try {
				_logger.LogDebug(debugNotify);

				// Telegram notifications
				if (config.EnableTelegram)
					await services.GetRequiredService<TgBot>().SendMessage(token: config.TelegramToken, chatID: config.TelegramChatID, pushList, htmlMode: true);
				else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark)
					await services.GetRequiredService<Barker>().SendMessage(config.BarkAddress, config.BarkToken, pushList);
				else _logger.LogInformation(debugDisabledFormat, "Bark");

				_logger.LogDebug($"Done: {debugNotify}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugNotify}");
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
