using DiscordRPC;
using Microsoft.Extensions.Logging;
using YoutubeMusicDiscordRichPresenceCSharp.Models;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public class SongPresenceHandler(ILogger<SongPresenceHandler> logger) : ISongPresenceHandler
{
    public RichPresence GetSongPresence(IServiceResource resource, CurrentPlayingInfo info)
    {
        return new RichPresence
        {
            Type = ActivityType.Listening,
            Details = GetPresenceDetails(info),
            Timestamps = GetPresenceTimestamps(info),
            Assets = GetPresenceAssets(resource, info),
            Buttons = GetPresenceButtons(resource, info).ToArray()
        };
    }

    private List<Button> GetPresenceButtons(IServiceResource resource, CurrentPlayingInfo info)
    {
        var buttons = new List<Button>(3);

        if (info.SongUrl != string.Empty)
        {
            logger.LogInformation("Adding listen on {0} button.", resource.Name);
            buttons.Add(new Button()
            {
                Label = $"Listen on {resource.Name}",
                Url = $"{info.SongUrl}",
            });
        }

        logger.LogInformation("Adding install client button.");

        buttons.Add(new Button
        {
            Label = "Music In Browser Detector",
            Url = "https://github.com/pkg-dot-zip/MusicInBrowserDetectorForDiscord",
        });

        if (buttons.Count > 2) throw new InvalidOperationException("RPC has a limit of 2 buttons!");
        return buttons;
    }

    private Assets GetPresenceAssets(IServiceResource resource, CurrentPlayingInfo info)
    {
        var assets = new Assets
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

    private Timestamps? GetPresenceTimestamps(CurrentPlayingInfo info)
    {
        if (info.IsPaused) return null;
        if (info.TimeInfo is null) return null;

        return new Timestamps
        {
            Start = DateTime.UtcNow.AddSeconds(-info.TimeInfo.CurrentTime),
            End = DateTime.UtcNow.AddSeconds(info.TimeInfo.RemainingTime)
        };
    }

    private string GetPresenceDetails(CurrentPlayingInfo info) => $"{info.MetaData?.Artist} - {info.MetaData?.Title}";
}