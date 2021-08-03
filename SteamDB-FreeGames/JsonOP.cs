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

		public JsonOP(ILogger<JsonOP> logger) {
			_logger = logger;
		}

		public void WriteData(List<Dictionary<string, string>> data) {
			try {
				if (data.Count > 0) {
					_logger.LogDebug("Writing records!");
					string json = JsonConvert.SerializeObject(data, Formatting.Indented);
					File.WriteAllText(recordPath, string.Empty);
					File.WriteAllText(recordPath, json);
					_logger.LogDebug("Done");
				} else _logger.LogDebug("No records detected, quit writing records");
			} catch (Exception) {
				_logger.LogError("Writing data failed.");
				throw;
			} finally {
				Dispose();
			}
		}

		public List<Dictionary<string, string>> LoadData() {
			try {
				_logger.LogDebug("Loading previous records");
				var content = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(File.ReadAllText(recordPath));
				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Loading previous records failed.");
				throw;
			}
		}

		public Dictionary<string, string> LoadConfig() {
			try {
				_logger.LogDebug("Loading config");
				var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(configPath));
				if (content["TOKEN"] == string.Empty) {
					throw new Exception(message: "No Token provided!");
				}
				if (content["CHAT_ID"] == string.Empty) {
					throw new Exception(message: "No ChatID provided!");
				}
				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Loading config failed.");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
