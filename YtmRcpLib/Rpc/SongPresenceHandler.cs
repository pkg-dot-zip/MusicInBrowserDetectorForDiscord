using DiscordRPC;
using YtmRcpLib.Models;

namespace YtmRcpLib.Rpc;

public static class SongPresenceHandler
{
    public static RichPresence GetSongPresence(CurrentPlayingInfo info)
    {
        return new RichPresence()
        {
            Type = ActivityType.Listening,
            Details = GetPresenceDetails(info),
            Timestamps = GetPresenceTimestamps(info),
            Assets = GetPresenceAssets(info),
            Buttons = GetPresenceButtons(info).ToArray()
        };
    }

    private static List<Button> GetPresenceButtons(CurrentPlayingInfo info)
    {
        var buttons = new List<Button>(3);

        if (info.SongUrl != string.Empty)
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
        var assets = new Assets()
        {
            LargeImageKey = $"{info.MetaData?.ArtworkUrl}",
            LargeImageText = $"{info.MetaData?.Album}",
        };

        if (info.IsPaused)
        {
            assets.SmallImageKey = "https://cdn-icons-png.flaticon.com/512/4181/4181163.png";
            assets.SmallImageText = "Paused";
        }
        else
        {
            assets.SmallImageKey =
                "https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/youtube-music-icon.png";
            assets.SmallImageText = "Playing";
        }

        return assets;
    }

    private static Timestamps? GetPresenceTimestamps(CurrentPlayingInfo info)
    {
        if (info.IsPaused) return null;
        if (info.TimeInfo is null) return null;

        return new Timestamps()
        {
            Start = DateTime.UtcNow.AddSeconds(-info.TimeInfo.CurrentTime),
            End = DateTime.UtcNow.AddSeconds(info.TimeInfo.RemainingTime)
        };
    }

    private static string GetPresenceDetails(CurrentPlayingInfo info) => $"{info.MetaData?.Artist} - {info.MetaData?.Title}";
}