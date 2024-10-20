using DiscordRPC;
using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
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
        var script = @"
            return navigator.mediaSession.metadata ? {
                title: navigator.mediaSession.metadata.title,
                artist: navigator.mediaSession.metadata.artist,
                album: navigator.mediaSession.metadata.album,
                artwork: navigator.mediaSession.metadata.artwork
            } : null;
        ";

        var result = (IJavaScriptExecutor)driver;
        var metadata = (Dictionary<string, object>)result.ExecuteScript(script);


        if (metadata != null)
        {
            Console.WriteLine($"Title: {metadata["title"]}");
            Console.WriteLine($"Artist: {metadata["artist"]}");
            Console.WriteLine($"Album: {metadata["album"]}");
        }
        else
        {
            Console.WriteLine("No song is currently playing or metadata is unavailable.");
        }

        RpcHandler.Initialize();

        RpcHandler.SetPresence(new RichPresence()
        {
            Details = "Example Project",
            State = "csharp example",
            Assets = new Assets()
            {
                LargeImageKey = "image_large",
                LargeImageText = "Lachee's Discord IPC Library",
                SmallImageKey = "image_small"
            },
        });

        Console.ReadKey();

        RpcHandler.Deinitialize();
    }
}