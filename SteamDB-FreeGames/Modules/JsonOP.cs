using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Modules {
	public class JsonOP: IDisposable {
		private readonly ILogger<JsonOP> _logger;

		#region path strings
		private readonly string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}Config File{Path.DirectorySeparatorChar}config.json";
		private readonly string recordsPath = $"{AppDomain.CurrentDomain.BaseDirectory}Records{Path.DirectorySeparatorChar}Records.json";
		#endregion

		#region debug strings
		private readonly string debugWrite = "Write records";
		private readonly string debugLoadConfig = "Load config";
		private readonly string debugLoadRecords = "Load previous records";
		#endregion

		public JsonOP(ILogger<JsonOP> logger) {
			_logger = logger;
		}

		public void WriteData(List<FreeGameRecord> data) {
			try {
				if (data.Count > 0) {
					_logger.LogDebug(debugWrite);
					string json = JsonConvert.SerializeObject(data, Formatting.Indented);
					File.WriteAllText(recordsPath, string.Empty);
					File.WriteAllText(recordsPath, json);
					_logger.LogDebug($"Done: {debugWrite}");
				} else _logger.LogDebug("No records detected, quit writing records");
			} catch (Exception) {
				_logger.LogError($"Error: {debugWrite}");
				throw;
			} finally {
				Dispose();
			}
		}

		public List<FreeGameRecord> LoadData() {
			try {
				_logger.LogDebug(debugLoadRecords);
				var content = JsonConvert.DeserializeObject<List<FreeGameRecord>>(File.ReadAllText(recordsPath));
				_logger.LogDebug($"Done: {debugLoadRecords}");
				return content;
			} catch (Exception) {
				_logger.LogError($"Error: {debugLoadRecords}");
				throw;
			}
		}

		public Config LoadConfig() {
			try {
				_logger.LogDebug(debugLoadConfig);
				var content = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
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
