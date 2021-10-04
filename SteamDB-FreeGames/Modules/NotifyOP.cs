using System;
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
		private readonly string debugEnabledFormat = "Sending notifications to {0}";
		private readonly string debugDisabledFormat = "{0} notify is disabled, skipping";
		private readonly string debugNoNewNotifications = "No new notifications! Skipping";
		private readonly string debugGenerateNotifyRecordList = "Generating notify record";
		private readonly string debugIsUpdateRecord = "{0} exists in previous record, marking as a update record";
		private readonly string debugIsNotUpdateRecord = "{0} NOT exist in previous record";
		#endregion


		public NotifyOP(ILogger<NotifyOP> logger) {
			_logger = logger;
		}

		private List<NotifyRecord> GenerateNotifyRecordList(List<FreeGameRecord> oldRecord, List<FreeGameRecord> pushList) {
			_logger.LogDebug(debugGenerateNotifyRecordList);
			var resultList = new List<NotifyRecord>();

			try {
				foreach (var record in pushList) {
					bool isUpdate = oldRecord.Exists(x => x.Name == record.Name || x.ID == record.ID);
					if (isUpdate) _logger.LogDebug(debugIsUpdateRecord, record.Name);
					else _logger.LogDebug(debugIsNotUpdateRecord, record.Name);
					resultList.Add(new NotifyRecord(record) { IsUpdate = isUpdate });
				}
			} catch (Exception) {
				_logger.LogError($"Error: {debugGenerateNotifyRecordList}");
				throw;
			}

			_logger.LogDebug($"Done: {debugGenerateNotifyRecordList}");
			return resultList;
		}

		public async Task Notify(NotifyConfig config, List<FreeGameRecord> oldRecord, List<FreeGameRecord> pushList) {
			if (pushList.Count == 0) {
				_logger.LogInformation(debugNoNewNotifications);
				return;
			}

			var pushListFinal = GenerateNotifyRecordList(oldRecord, pushList);

			try {
				_logger.LogDebug(debugNotify);

				// Telegram notifications
				if (config.EnableTelegram) {
					_logger.LogInformation(debugEnabledFormat, "Telegram");
					await services.GetRequiredService<TgBot>().SendMessage(config, pushListFinal);
				} else _logger.LogInformation(debugDisabledFormat, "Telegram");

				// Bark notifications
				if (config.EnableBark) {
					_logger.LogInformation(debugEnabledFormat, "Bark");
					await services.GetRequiredService<Barker>().SendMessage(config, pushListFinal);
				} else _logger.LogInformation(debugDisabledFormat, "Bark");

				// QQ notifications
				if (config.EnableQQ) {
					_logger.LogInformation(debugEnabledFormat, "QQ");
					await services.GetRequiredService<QQPusher>().SendMessage(config, pushListFinal);
				} else _logger.LogInformation(debugDisabledFormat, "QQ");

				// PushPlus notifications
				if (config.EnablePushPlus) {
					_logger.LogInformation(debugEnabledFormat, "PushPlus");
					await services.GetRequiredService<PushPlus>().SendMessage(config, pushListFinal);
				} else _logger.LogInformation(debugDisabledFormat, "PushPlus");

				// DingTalk notifications
				if (config.EnableDingTalk) {
					_logger.LogInformation(debugEnabledFormat, "DingTalk");
					await services.GetRequiredService<DingTalk>().SendMessage(config, pushListFinal);
				} else _logger.LogInformation(debugDisabledFormat, "DingTalk");

				// Email notifications
				if (config.EnableEmail) {
					_logger.LogInformation(debugEnabledFormat, "Email");
					await services.GetRequiredService<Email>().SendMessage(config, pushListFinal);
				} else _logger.LogInformation(debugDisabledFormat, "Email");

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
