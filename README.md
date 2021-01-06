# SteamDB-FreeGames-dotnet
 [SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames) dotnet version

**Seems that SteamDB really don't want people scraping their site.**

## Requirements

- .NET 5	
    - HtmlAgilityPack
    - ScrapySharp
    - PlaywrightSharp
    - Telegram.Bot

## Build

### Command

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64]
```

publish as a single file

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

and enable compression if you want to

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=true
```

## Usage

Before executing the binary file, please follow these steps:

1. Create a config.json file and a record.json file in the same directory.

2. Add these variables in config.json file

   ```json
   {
   	"TOKEN": "",
   	"CHAT_ID": ""
   }
   ```
   

Then fill your Telegram Bot token and your account's Chat ID.

3. Add a "[]"(empty bracket, as a empty list in json format) in record.json file, otherwise the program will throw a null object error.
4. Download chromedirver.exe file in the repo, and place it in your PATH directory. 

Tested on Windows Server 2019, macOS Catalina 10.15.6.

