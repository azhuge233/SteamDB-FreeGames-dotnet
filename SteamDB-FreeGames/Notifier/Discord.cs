using Microsoft.Extensions.Logging;
using SteamDB_FreeGames.Models;
using System;
using System.Collections.Generic;
using SteamDB_FreeGames.Models.PostContent;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using Microsoft.Playwright;
using System.Runtime.Intrinsics.X86;

namespace SteamDB_FreeGames.Notifier {
	public class Discord: INotifiable {
		private readonly ILogger<Discord> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Discord";
		private readonly string debugSendMessageASF = "Send ASF result to Discord";
		private readonly string debugGeneratePostContent = "Generating Discord POST content";
		#endregion

		public Discord(ILogger<Discord> logger) {
			_logger = logger;
		}

		private DiscordPostContent GeneratePostContent(List<NotifyRecord> records) {
			_logger.LogDebug(debugGeneratePostContent);

			var ids = string.Join(',', records.Select(rec => rec.ID));
			var content = new DiscordPostContent() {
				Content = records.Count > 1 ? "New Free Games - SteamDB" : "New Free Game - SteamDB"
			};

			foreach (var record in records) {
				content.Embeds.Add(
					new Embed() {
						Title = record.Name,
						Url = record.Url,
						Description = record.ToDiscordMessage(update: record.IsUpdate),
						Footer = new Footer() {
							Text = NotifyFormatStrings.projectLink
						}
					}
				);
			}
			content.Embeds.Add(new Embed() { 
				Description = ids, Color = 12960704
			});

			_logger.LogDebug($"Done: {debugGeneratePostContent}");
			return content;
		}

		public async Task SendMessage(NotifyConfig config, List<NotifyRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = config.DiscordWebhookURL;
				var content = GeneratePostContent(records);

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				var resp = await new HttpClient().PostAsync(url, data);
				_logger.LogDebug(await resp.Content.ReadAsStringAsync());

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		public async Task SendMessage(NotifyConfig config, string asfResult) {
			try {
				_logger.LogDebug(debugSendMessageASF);

				var url = config.DiscordWebhookURL;
				var content = new DiscordPostContent() {
					Content = "ASF Result",
				};

				content.Embeds.Add(
					new Embed() { 
						Description = asfResult,
						Color = 11282734
					}
				);

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				var resp = await new HttpClient().PostAsync(url, data);

				_logger.LogDebug($"Done: {debugSendMessageASF}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessageASF}");
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
