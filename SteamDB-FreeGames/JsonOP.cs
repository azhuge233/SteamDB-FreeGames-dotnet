using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace SteamDB_FreeGames {
	public class JsonOP: IDisposable {
		private readonly ILogger<JsonOP> _logger;
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}config.json";
		private readonly string recordPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}record.json";

		private readonly string debugWrite = "Write records";
		private readonly string debugLoadConfig = "Load config";
		private readonly string debugLoadRecords = "Load previous records";

		public JsonOP(ILogger<JsonOP> logger) {
			_logger = logger;
		}

		public void WriteData(List<Dictionary<string, string>> data) {
			try {
				if (data.Count > 0) {
					_logger.LogDebug(debugWrite);
					string json = JsonConvert.SerializeObject(data, Formatting.Indented);
					File.WriteAllText(recordPath, string.Empty);
					File.WriteAllText(recordPath, json);
					_logger.LogDebug($"Done: {debugWrite}");
				} else _logger.LogDebug("No records detected, quit writing records");
			} catch (Exception) {
				_logger.LogError($"Error: {debugWrite}");
				throw;
			} finally {
				Dispose();
			}
		}

		public List<Dictionary<string, string>> LoadData() {
			try {
				_logger.LogDebug(debugLoadRecords);
				var content = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(File.ReadAllText(recordPath));
				_logger.LogDebug($"Done: {debugLoadRecords}");
				return content;
			} catch (Exception) {
				_logger.LogError($"Error: {debugLoadRecords}");
				throw;
			}
		}

		public Dictionary<string, string> LoadConfig() {
			try {
				_logger.LogDebug(debugLoadConfig);
				var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(configPath));
				if (content["TOKEN"] == string.Empty) {
					throw new Exception(message: "No Token provided!");
				}
				if (content["CHAT_ID"] == string.Empty) {
					throw new Exception(message: "No ChatID provided!");
				}
				_logger.LogDebug($"Done: {debugLoadConfig}");
				return content;
			} catch (Exception) {
				_logger.LogError($"Error: {debugLoadConfig}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
