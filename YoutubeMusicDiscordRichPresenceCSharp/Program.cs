using BrowserLib.Browser;
using YtmRcpLib.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    public static void Main(string[] args)
    {
        if (!ChromeHandler.IsChromeRunningWithRemoteDebugging().Result)
        {
            ChromeHandler.OpenChromeBrowserWithRemoteDebugging();
        }

        var driver = ChromeHandler.GetChromeInstance();

        Thread.Sleep(5000);

        var playingInfo = SongRetriever.FromBrowser(driver);
        
        RpcHandler.Initialize();

        if (playingInfo is not null)
        {
            RpcHandler.SetPresence(SongPresenceHandler.GetSongPresence(playingInfo));
        }
        else
        {
            Console.WriteLine("Couldn't find song, thus no rpc set.");
        }

        Console.ReadKey();

        RpcHandler.Deinitialize();
    }
}