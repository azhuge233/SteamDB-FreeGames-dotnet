# SteamDB-FreeGames-dotnet
 [SteamDB-FreeGames](https://github.com/azhuge233/SteamDB-FreeGames) dotnet core version

### Note that this project is no longer usable, but I may update it once it's achieveable. Read more abhout this at [python version readme](https://github.com/azhuge233/SteamDB-FreeGames).

##Requirements

- dotnet core 3.1
- 2captcha account
  - SteamDB now with hcaptcha, the program is using 2captcha to bypass it.

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

