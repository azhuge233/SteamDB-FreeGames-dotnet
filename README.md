# SteamDB-FreeGames-dotnet
 [SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames) dotnet core version

## How this works

In python version, I use undetected_chromedriver to guise selenium bot in order to bypass cloudflare's 5 secs anti-bot page. In C#, there's no similar package to do that, but instead of using undetected_chromedriver, I used the "hacked" chromedriver that undetected_chromedriver patched, then added two arguements and the whole thing works.

## Requirements

- dotnet core 3.1

## Build

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
   	"CHAT_ID": ""
   }
   ```
   

Then fill your Telegram Bot token, your account's Chat ID and your 2captcha API key.

3. Add a "[]"(empty bracket, as a empty list in json format) in record.json file, otherwise the program will throw a null object error.
4. Download chromedirver.exe file in the repo, and place it in your PATH directory. 

Tested on Windows Server 2016, macOS Catalina 10.15.6.

