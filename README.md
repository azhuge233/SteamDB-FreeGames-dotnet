# SteamDB-FreeGames-dotnet
 [SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames) dotnet version

**Seems that SteamDB really don't want people scraping their site, check [Things should be aware of](https://github.com/azhuge233/SteamDB-FreeGames-dotnet/blob/master/Things%20should%20be%20aware%20of.md) before using.**

## My Free Games Collection

- SteamDB
    - [https://github.com/azhuge233/SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames)(Python)
    - [https://github.com/azhuge233/SteamDB-FreeGames-dotnet](https://github.com/azhuge233/SteamDB-FreeGames-dotnet)
- EpicBundle
    - [https://github.com/azhuge233/EpicBundle-FreeGames](https://github.com/azhuge233/EpicBundle-FreeGames)(Python)
    - [https://github.com/azhuge233/EpicBundle-FreeGames-dotnet](https://github.com/azhuge233/EpicBundle-FreeGames-dotnet)
- Indiegala
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)
- GOG
    - [https://github.com/azhuge233/IndiegalaFreebieNotifier](https://github.com/azhuge233/IndiegalaFreebieNotifier)

## Requirements

- .NET 5	
    - NLog
    - HtmlAgilityPack
    - ScrapySharp
    - PlaywrightSharp
    - Telegram.Bot

## Build

### Publish

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64]
```

~~publish as a single file~~

~~```~~
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
~~```~~

~~and enable compression if you want to~~

~~```~~
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] -p:PublishTrimmed=true
~~```~~

 Currently, publishing as a single file will make playwright dysfunctional.

## Usage

Fill your Telegram Bot token and chat ID in config.json

```json
{
	"TOKEN": "xxxxxx:xxxxxx",
	"CHAT_ID": "xxxxxxxx"
}
```

Tested on Windows Server 2019, macOS Catalina 10.15.6.
