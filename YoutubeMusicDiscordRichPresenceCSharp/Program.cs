﻿using BrowserLib.Browser;
using YtmRcpLib.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    public static void Main(string[] args)
    {
        Run(new ChromeHandler());
    }

    private static void Run(IBrowser browserHandler)
    {
        if (!browserHandler.IsRunning())
        {
            browserHandler.OpenWindow();
        }

        var driver = browserHandler.GetDriver();

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
        browserHandler.Close();
    }
}