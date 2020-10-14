# SteamDB-FreeGames-dotnet
 [SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames) dotnet core version

### Python version now uses undetected_chromedriver to bypass cloudflare's new anti-bot page, but I haven't found a same package in C#, I will update this dotnet version ASAP.

##Requirements

- dotnet core 3.1

##Build

### Command

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64]
```

publish as a single file

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] -p:PublishSingleFile=true
```

and enable compression if you want to

```
dotnet publish -c Release -o /your/path/here -r [win10-x64/osx-x64/linux-x64] -p:PublishSingleFile=true -p:PublishTrimmed=true
```

## Usage

Before executing the binary file, please follow these steps:

1. Create a config.json file and a record.json file in the same directory.

2. Add these variables in config.json file

   ```json
   {
   	"TOKEN": "",
   	"CHAT_ID": "",
   	"API_KEY": ""
   }
   ```

   Then fill your Telegram Bot token, your account's Chat ID and your 2captcha API key.

3. Add a "[]"(empty bracket, as a empty list in json format) in record.json file, otherwise the program will throw a null object error.

Tested on Windows Server 2016, macOS Catalina 10.15.6.

