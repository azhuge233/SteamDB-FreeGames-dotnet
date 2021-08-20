# SteamDB-FreeGames-dotnet

Fetch free games info from SteamDB, send notifications through Telegram, Email, Bark, QQ, PushPlus and DingTalk.

Demo Telegram Channel [@azhuge233_FreeGames](https://t.me/azhuge233_FreeGames)

**Seems that SteamDB really don't want people scraping their site, check [Things should be aware of](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/blob/master/Things%20should%20be%20aware%20of.md) before using.**

## Build

Install dotnet 5.0 SDK first, you can find installation packages/guides [here](https://dotnet.microsoft.com/download).

### Publish

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64]
```

## Usage

Fill your Telegram Bot token and chat ID in config.json

```json
{
	"TelegramToken": "xxxxxx:xxxxxx",
	"TelegramChatID": "xxxxxxxx",
}
```

Check [wiki](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/wiki/Config-Description) for config variable descriptions.

Tested on Windows Server 2019, macOS Catalina 10.15.6.

## My Free Games Collection

- SteamDB
    - [https://github.com/azhuge233/SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames)(Archived)
    - [https://github.com/azhuge233/SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet)
- EpicBundle
    - [https://github.com/azhuge233/EpicBundle-FreeGames](https://github.com/azhuge233/EpicBundle-FreeGames)(Archived)
    - [https://github.com/azhuge233/EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)
- Indiegala
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)
- GOG
    - [https://github.com/azhuge233/GOGGiveawayNotifier](https://github.com/azhuge233/GOGGiveawayNotifier)
