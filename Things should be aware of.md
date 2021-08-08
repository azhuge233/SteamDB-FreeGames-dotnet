### Few things you should be aware of.

1. About scraping frequency

    - SteamDB has been trying things to protect their site from scraping, include adding fake record and hCaptcha, which is disgusting, this action has also affected non-scraping people's experience, you can search for complaints at [keylol.com](https://keylol.com) (or some other steam community sites).

        So for the long-term interests (both scraping and visiting), setting a proper delay is important(I personally set one request per 15 minutes), and to be honest, the free games page is not that active since Steam itself does not always has new free games.
        
    - If you access SteamDB too frequently, Cloudflare will set higher challenge difficulty. If the frequency doesn't slow down, eventually Cloudflare will mark your IP (or some other signatures) and block your requests for a while, the browser will be at least stuck in a js-challenge loop. I'm not promising this is the actual Cloudflare strategy, but this is what I've experienced.

2. About SteamDB free games page

    - Free games' start time that SteamDB provides is not precise, when the script gets a new game then send it to Telegram, the free time probably isn't started, it's normal to be failed when you instantly add free games to your account.
    - SteamDB does not always has the first-hand information, some other source should be added if you really want to get all the free games.