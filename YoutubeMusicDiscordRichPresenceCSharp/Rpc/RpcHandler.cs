using DiscordRPC.Logging;
using DiscordRPC;
using YoutubeMusicDiscordRichPresenceCSharp.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

internal static class RpcHandler
{
    private static readonly DiscordRpcClient Client = new DiscordRpcClient("1297469080273420329");

    public static void Initialize()
    {
        //Set the logger
        Client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

        //Subscribe to events
        Client.OnReady += (sender, e) =>
        {
            Console.WriteLine("Received Ready from user {0}", e.User.Username);
        };

        Client.OnPresenceUpdate += (sender, e) =>
        {
            Console.WriteLine("Received Update! {0}", e.Presence);
        };

        //Connect to the RPC
        Client.Initialize();
    }

    // RpcHandler.SetPresence(new RichPresence()
    // {
    //     Details = "Example Project",
    //     State = "csharp example",
    //     Assets = new Assets()
    //     {
    //         LargeImageKey = "image_large",
    //         LargeImageText = "Lachee's Discord IPC Library",
    //         SmallImageKey = "image_small"
    //     },
    // });

    public static void SetSongPresence(CurrentPlayingInfo info)
    {
        SetPresence(new RichPresence()
        {
            Details = $"{info.Artist} - {info.Title}",
            State = $"{info.Album}",
            Assets = new Assets()
            {
                LargeImageKey = $"{info.ArtworkUrl}",
                LargeImageText = "Lachee's Discord IPC Library",
                SmallImageKey = "image_small"
            },
        });
    }

    public static void SetPresence(RichPresence presence)
    {
        Client.SetPresence(presence);
    }

    public static void Deinitialize()
    {
        Client.Dispose();
    }
}