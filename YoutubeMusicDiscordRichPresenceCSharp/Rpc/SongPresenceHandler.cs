using DiscordRPC;
using YoutubeMusicDiscordRichPresenceCSharp.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

internal static class SongPresenceHandler
{
    private static List<Button> GetPresenceButtons(CurrentPlayingInfo info)
    {
        List<Button> buttons = new List<Button>(3);

        if (info.SongUrl is not null && info.SongUrl != string.Empty) // TODO: Can not be null. Remove!
        {
            Console.WriteLine("Adding listen on YT button.");
            buttons.Add(new Button()
            {
                Label = "Listen on Youtube Music",
                Url = $"{info.SongUrl}",
            });
        }

        Console.WriteLine("Adding install client button.");

        buttons.Add(new Button()
        {
            Label = "Install YTM RPC Client",
            Url = "https://github.com/pkg-dot-zip/YoutubeMusicDiscordRichPresenceCSharp",
        });

        if (buttons.Count > 2) throw new InvalidOperationException("RPC has a limit of 2 buttons!");
        return buttons;
    }

    private static Assets GetPresenceAssets(CurrentPlayingInfo info)
    {
        return new Assets()
        {
            LargeImageKey = $"{info.ArtworkUrl}",
            LargeImageText = $"{info.Album}",
            SmallImageKey =
                "https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/youtube-music-icon.png",
        };
    }

    private static Timestamps GetPresenceTimestamps(CurrentPlayingInfo info)
    {
        return new Timestamps()
        {
            Start = DateTime.UtcNow.AddSeconds(-info.DurationTime) // TODO: Fix. This time is wrong!
        };
    }

    private static string GetPresenceDetails(CurrentPlayingInfo info)
    {
        return $"{info.Artist} - {info.Title}";
    }

    public static RichPresence GetSongPresence(CurrentPlayingInfo info)
    {
        // TODO: Change type to listening once it is released in the new version of rpc c#.
        return new RichPresence()
        {
            Details = GetPresenceDetails(info),
            Timestamps = GetPresenceTimestamps(info),
            Assets = GetPresenceAssets(info),
            Buttons = GetPresenceButtons(info).ToArray()
        };
    }
}