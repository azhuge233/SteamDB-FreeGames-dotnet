using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SteamDB_FreeGames {
	public class JsonOP: IDisposable {

		public async Task WriteData(List<Dictionary<string, string>> data, string path) {
			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			await Task.Run(() => {
				File.WriteAllText(path, string.Empty);
				File.WriteAllText(path, json);
			});
		}

		public List<Dictionary<string, string>> LoadData(string path) {
			Thread.Sleep(3000);
			var content = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
		}

		public Dictionary<string, string> LoadConfig(string path) {
			var content = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
		}

		public void Dispose() { }
	}
}
