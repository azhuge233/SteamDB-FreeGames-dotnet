### Few things you should be aware of.

1. The scraping frequency

    - SteamDB has been trying things to protect their site from scraping, include adding fake record and hCaptcha, which is disgusting, this action has also affected non-scraping people's experience, you can search for compliants at [keylol.com](https://keylol.com) (or some other sites).

        So for the long-term interests (both scraping and visiting), setting a proper delay is important, and to be honest, the free games page is not that active since Steam itself does not always has new free games.
        
    - If you access SteamDB too frequently, Cloudflare will mark your IP (or other signatures) and block your requests for a while.

2. SteamDB's free games page

    - Free games' start time that SteamDB provides is not precise, when the script gets a new game then send it to Telegram, the free time probably isn't started, it's normal to be failed when you instantly add free games to your account.
    - SteamDB does not always has the first-hand information, some other source should be added if you really want to get all the free games.