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

    public static void SetSongPresence(CurrentPlayingInfo info)
    {
        SetPresence(new RichPresence()
        {
            Details = $"{info.Artist} - {info.Title}",
            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow.AddSeconds(-info.DurationTime) // TODO: Fix. This time is wrong!
            },
            Assets = new Assets()
            {
                LargeImageKey = $"{info.ArtworkUrl}",
                LargeImageText = $"{info.Album}",
                SmallImageKey = "https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/youtube-music-icon.png",
            },
            Buttons = new Button[]
            {
                new Button()
                {
                    Label = "Install YTM RPC Client",
                    Url = "https://github.com/pkg-dot-zip/YoutubeMusicDiscordRichPresenceCSharp",
                }
            }
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