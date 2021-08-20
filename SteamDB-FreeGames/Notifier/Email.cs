using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class Email: INotifiable {
		private readonly ILogger<Email> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Email";
		private readonly string debugCreateMessage = "Create notification message";
		#endregion

		public Email(ILogger<Email> logger) {
			_logger = logger;
		}

		private MimeMessage CreateMessage(List<FreeGameRecord> pushList, string fromAddress, string toAddress) {
			try {
				_logger.LogDebug(debugCreateMessage);

				var message = new MimeMessage();

				message.From.Add(new MailboxAddress("SteamDB-FreeGames", fromAddress));
				message.To.Add(new MailboxAddress("Receiver", toAddress));

				var sb = new StringBuilder();
				var sbSubID = new StringBuilder();

				message.Subject = sb.AppendFormat(NotifyFormatStrings.emailTitleFormat, pushList.Count).ToString();
				sb.Clear();

				pushList.ForEach(record => {
					sbSubID.Append(sbSubID.Length == 0 ? record.ID : $",{record.ID}");
					sb.AppendFormat(NotifyFormatStrings.emailBodyFormat, record.ToEmailMessage());
				});

				message.Body = new TextPart("html") {
					Text = sbSubID.Append("<br>")
						.Append(sb)
						.Append(NotifyFormatStrings.projectLinkHTML)
						.ToString()
				};
		
				_logger.LogDebug($"Done: {debugCreateMessage}");
				return message;
			} catch (Exception) {
				_logger.LogError($"Error: {debugCreateMessage}");
				throw;
			}
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var message = CreateMessage(records, config.FromEmailAddress, config.ToEmailAddress);

				using var client = new SmtpClient();
				client.Connect(config.SMTPServer, config.SMTPPort, true);
				client.Authenticate(config.AuthAccount, config.AuthPassword);
				await client.SendAsync(message);
				client.Disconnect(true);

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
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
