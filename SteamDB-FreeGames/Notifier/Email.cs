﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	class Email:IDisposable {
		private readonly ILogger<Email> _logger;

		#region message format
		private readonly string titleFormat = "{0} new free game(s) - SteamDB-FreeGames";
		private readonly string bodyFormat = "<br>{0}";
		#endregion

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

				message.Subject = sb.AppendFormat(titleFormat, pushList.Count).ToString();
				sb.Clear();

				pushList.ForEach(record => {
					sbSubID.Append(sbSubID.Length == 0 ? record.ID : $",{record.ID}");
					sb.AppendFormat(bodyFormat, record.ToEmailMessage());
				});

				message.Body = new TextPart("html") {
					Text = sbSubID.Append("<br>").Append(sb).ToString()
				};
		
				_logger.LogDebug($"Done: {debugCreateMessage}");
				return message;
			} catch (Exception) {
				_logger.LogError($"Error: {debugCreateMessage}");
				throw;
			}
		}

		public async Task SendMessage(string fromAddress, string toAddress, string smtpServer, int smtpPort, string authAccount, string authPassword, List<FreeGameRecord> pushList) {
			try {
				_logger.LogDebug(debugSendMessage);

				var message = CreateMessage(pushList, fromAddress, toAddress);

				using var client = new SmtpClient();
				client.Connect(smtpServer, smtpPort, true);
				client.Authenticate(authAccount, authPassword);
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
