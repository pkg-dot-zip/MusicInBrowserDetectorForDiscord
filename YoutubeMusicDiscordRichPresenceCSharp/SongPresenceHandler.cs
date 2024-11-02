using DiscordRPC;
using YoutubeMusicDiscordRichPresenceCSharp.Services;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public static class SongPresenceHandler
{
    public static RichPresence GetSongPresence(IServiceResource resource, CurrentPlayingInfo info)
    {
        return new RichPresence()
        {
            Type = ActivityType.Listening,
            Details = GetPresenceDetails(info),
            Timestamps = GetPresenceTimestamps(info),
            Assets = GetPresenceAssets(resource, info),
            Buttons = GetPresenceButtons(resource, info).ToArray()
        };
    }

    private static List<Button> GetPresenceButtons(IServiceResource resource, CurrentPlayingInfo info)
    {
        var buttons = new List<Button>(3);

        if (info.SongUrl != string.Empty)
        {
            Console.WriteLine("Adding listen on 'platform' button.");
            buttons.Add(new Button()
            {
                Label = $"Listen on {resource.Name}",
                Url = $"{info.SongUrl}",
            });
        }

        Console.WriteLine("Adding install client button.");

        buttons.Add(new Button()
        {
            Label = "Install Music In Browser Detector",
            Url = "https://github.com/pkg-dot-zip/YoutubeMusicDiscordRichPresenceCSharp",
        });

        if (buttons.Count > 2) throw new InvalidOperationException("RPC has a limit of 2 buttons!");
        return buttons;
    }

    private static Assets GetPresenceAssets(IServiceResource resource, CurrentPlayingInfo info)
    {
        var assets = new Assets()
        {
            LargeImageKey = $"{info.MetaData?.ArtworkUrl}",
            LargeImageText = $"{info.MetaData?.Album}",
        };

        if (info.IsPaused)
        {
            assets.SmallImageKey = resource.PausedIconKey;
            assets.SmallImageText = $"Paused {resource.Name}";
        }
        else
        {
            assets.SmallImageKey =
                resource.PlayingIconKey;
            assets.SmallImageText = $"Playing {resource.Name}";
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