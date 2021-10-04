using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SteamDB_FreeGames.Models;

namespace SteamDB_FreeGames.Notifier {
	interface INotifiable: IDisposable {
		public Task SendMessage(NotifyConfig config, List<NotifyRecord> records);
	}
}
