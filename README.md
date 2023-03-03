# SteamDB-FreeGames-dotnet

### Update: 
I've already found a simple, no browser extensions reqired solution to solve the captcha, it's been runnning for months with no critical error, but I'm still not planning to update this repo.

## No longer maintained, head [here](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/wiki/No-longer-maintained) for more information.

A CLI tool that
 - Fetches free games info from SteamDB
 - Sends notifications through Telegram, Email, Bark, QQ, PushPlus, DingTalk, PushDeer and Discord
 - Auto claim fetched free games with ASF IPC

Demo Telegram Channel [@azhuge233_FreeGames](https://t.me/azhuge233_FreeGames)

**Seems that SteamDB really don't want people scraping their site, check [Things should be aware of](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/blob/master/Things%20should%20be%20aware%20of.md) before using.**

## Build

Install dotnet 6.0 SDK first, you can find installation packages/guides [here](https://dotnet.microsoft.com/download).

### Publish

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] --sc
```

## Usage

Set your Telegram Bot token and chat ID in config.json

```json
{
	"TelegramToken": "xxxxxx:xxxxxx",
	"TelegramChatID": "xxxxxxxx",
}
```

Check [wiki](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/wiki/Config-Description) for more notification method descriptions.

### ASF Auto Claim

To use auto claim, you have to set up ASF IPC server first, you can find the instructions [here](https://github.com/JustArchiNET/ArchiSteamFarm/wiki/IPC).

Set `EnableASF` to `true` to turn on auto claim, then fill your IPC address in `ASFIPCUrl`

```json
{
	"EnableASF":  true,
  	"ASFIPCUrl": "https://my.domain.com or with IP address and port number(didn't test)",
}
```

Auto claim uses "addlicense asf SubID/AppID" as the default addlicense command, if you have any customized prefix or ASF just simply returns "wrong command" kind of message, you may need to manually change the [command string](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/blob/7c682b8078a87464af2cbb5f2efd33446386a464/SteamDB-FreeGames/Models/String/ASFStrings.cs#L5) in [Models/String/ASFStrings.cs](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/blob/7c682b8078a87464af2cbb5f2efd33446386a464/SteamDB-FreeGames/Models/String/ASFStrings.cs).

Check [wiki](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/wiki/Config-Description) for more ASF related config variables descriptions.

### Repeatedly running

The program will not add while/for loop, it's a scraper. To schedule the program, use cron.d in Linux(macOS) or Task Scheduler in Windows.

Tested on Windows Server 2019/2022, macOS Catalina 10.15.6.

## My Free Games Collection

- SteamDB
    - [https://github.com/azhuge233/SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames)(Archived)
    - [https://github.com/azhuge233/SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet)(Not maintained)
- EpicBundle
    - [https://github.com/azhuge233/EpicBundle-FreeGames](https://github.com/azhuge233/EpicBundle-FreeGames)(Archived)
    - [https://github.com/azhuge233/EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)
- Indiegala
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)
- GOG
    - [https://github.com/azhuge233/GOGGiveawayNotifier](https://github.com/azhuge233/GOGGiveawayNotifier)
- Ubisoft
    - [https://github.com/azhuge233/UbisoftGiveawayNotifier](https://github.com/azhuge233/UbisoftGiveawayNotifier)
- PlayStation Plus
    - [https://github.com/azhuge233/PSPlusMonthlyGames-Notifier](https://github.com/azhuge233/PSPlusMonthlyGames-Notifier)
- Reddit community
    - [https://github.com/azhuge233/RedditFreeGamesNotifier](https://github.com/azhuge233/RedditFreeGamesNotifier)
