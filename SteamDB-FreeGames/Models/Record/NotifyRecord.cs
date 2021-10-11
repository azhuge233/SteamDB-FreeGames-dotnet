using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamDB_FreeGames.Models {
	public class NotifyRecord : FreeGameRecord {
		public bool IsUpdate { get; set; }

		public NotifyRecord(FreeGameRecord parentRecord) {
			ID = parentRecord.ID;
			Name = parentRecord.Name;
			FreeType = parentRecord.FreeType;
			Url = parentRecord.Url;
			StartTime = parentRecord.StartTime;
			EndTime = parentRecord.EndTime;

			IsUpdate = false;
		}

		private static string RemoveSpecialCharacters(string str) {
			return Regex.Replace(str, ParseStrings.removeSpecialCharsRegex, string.Empty);
		}

		public string ToTelegramMessage(bool update) {
			return new StringBuilder().AppendFormat(update ? NotifyFormatStrings.telegramUpdatePushFormat : NotifyFormatStrings.telegramPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString(), RemoveSpecialCharacters(Name)).ToString();
		}

		public string ToBarkMessage(bool update) {
			return new StringBuilder().AppendFormat(update ? NotifyFormatStrings.barkUpdatePushFormat : NotifyFormatStrings.barkPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToEmailMessage(bool update) {
			return new StringBuilder().AppendFormat(update ? NotifyFormatStrings.emailUpdatePushHtmlFormat : NotifyFormatStrings.emailPushHtmlFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToQQMessage(bool update) {
			return new StringBuilder().AppendFormat(update ? NotifyFormatStrings.qqUpdatePushFormat : NotifyFormatStrings.qqPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToPushPlusMessage(bool update) {
			return new StringBuilder().AppendFormat(update ? NotifyFormatStrings.pushPlusUpdatePushHtmlFormat : NotifyFormatStrings.pushPlusPushHtmlFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}

		public string ToDingTalkMessage(bool update) {
			return new StringBuilder().AppendFormat(update ? NotifyFormatStrings.dingTalkUpdatePushFormat : NotifyFormatStrings.dingTalkPushFormat, Name, ID, FreeType, Url, StartTime.ToString(), EndTime.ToString()).ToString();
		}
	}
}
