using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SteamDB_FreeGames.Models;
using SteamDB_FreeGames.Models.ASFOP;
using SteamDB_FreeGames.Models.String;

namespace SteamDB_FreeGames.Modules {
	internal class ASFOP: IDisposable {
		private readonly ILogger<ASFOP> _logger;

		#region debug strings
		private readonly string debugASFOP = "ASFOP";
		private readonly string debugGenerateSubIDString = "GenerateSubIDString";
		private readonly string infoAddlicenseResult = "Addlicense result: \n";
		private readonly string infoNoRecords = "No new record, skipping addlicense";
		#endregion

		public ASFOP(ILogger<ASFOP> logger) {
			_logger = logger;
		}

		private string GenerateSubIDString(List<FreeGameRecord> gameList) {
			try {
				_logger.LogDebug(debugGenerateSubIDString);

				StringBuilder sb = new();
				gameList.ForEach(game => sb.Append(sb.Length == 0 ? game.ID : $",{game.ID}"));

				_logger.LogDebug($"Done: {debugGenerateSubIDString}");
				return sb.ToString();
			} catch (Exception) {
				_logger.LogError($"Error: {debugGenerateSubIDString}");
				throw;
			}
		}

		public async Task<string> Addlicense(Config config, List<FreeGameRecord> gameList) {
			if (gameList.Count == 0) {
				_logger.LogInformation(infoNoRecords);
				return string.Empty;
			}

			try {
				_logger.LogDebug(debugASFOP);

				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Authentication", config.ASFIPCPassword);

				var url = new StringBuilder().AppendFormat(ASFStrings.commandUrl, config.ASFIPCUrl).ToString();
				var content = new StringContent(JsonConvert.SerializeObject( new AddlicenssPostContent() { Command = $"{ASFStrings.addlicenseCommand}{GenerateSubIDString(gameList)}" } ), Encoding.UTF8, "application/json");

				var response = await client.PostAsync(url, content);
				_logger.LogDebug(response.ToString());
				var responseContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync())[ASFStrings.addlicenseResponseResultKey];
				_logger.LogInformation($"{infoAddlicenseResult}{responseContent}");

				_logger.LogDebug($"Done: {debugASFOP}");
				return responseContent;
			} catch (Exception) {
				_logger.LogError($"Error: {debugASFOP}");
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
