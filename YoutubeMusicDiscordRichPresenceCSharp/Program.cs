using DiscordRPC;
using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YoutubeMusicDiscordRichPresenceCSharp.Models;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;

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

        var playingInfo = CurrentPlayingInfo.FromBrowser(driver);
        
        RpcHandler.Initialize();

        if (playingInfo is not null)
        {
            RpcHandler.SetSongPresence(playingInfo);
        }
        else
        {
            Console.WriteLine("Couldn't find song, thus no rpc set.");
        }

        Console.ReadKey();

        RpcHandler.Deinitialize();
    }
}