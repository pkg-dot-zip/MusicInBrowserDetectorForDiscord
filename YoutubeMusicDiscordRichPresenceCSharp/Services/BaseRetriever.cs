using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

public abstract class BaseRetriever : IServiceRetriever, IServiceResource
{
    public abstract string Name { get; }
    public abstract string Url { get; }
    public abstract string PlayingIconKey { get; }
    public abstract string PausedIconKey { get; }

    public CurrentPlayingInfo? FromBrowser(IBrowser browser)
    {
        if (!GetWindowUrl(browser.GetDriver()).StartsWith(Url))
        {
            Console.Out.WriteLine($"Not on {Name} anymore!");
            return null;
        }

        var driver = browser.GetDriver();

        var playingInfo = new CurrentPlayingInfo
        {
            MetaData = GetMetaData(driver),
            IsPaused = GetPauseState(driver),
            TimeInfo = GetTimeInfo(driver),
            SongUrl = GetSongUrl(driver)
        };

        if (!playingInfo.IsNothing()) return playingInfo;

        Console.WriteLine("No song is currently playing or metadata is unavailable.");
        return null;
    }

    public virtual MetaData GetMetaData(WebDriver driver)
    {
        const string metadataScript = """
                                                  return navigator.mediaSession.metadata ? {
                                                      title: navigator.mediaSession.metadata.title,
                                                      artist: navigator.mediaSession.metadata.artist,
                                                      album: navigator.mediaSession.metadata.album,
                                                      artwork: navigator.mediaSession.metadata.artwork[0].src
                                                  } : null;
                                      """;

        var retrieval = (Dictionary<string, object>)driver.ExecuteScript(metadataScript);

        Console.WriteLine($"Title: {retrieval["title"]}");
        Console.WriteLine($"Artist: {retrieval["artist"]}");
        Console.WriteLine($"Album: {retrieval["album"]}");

        return new MetaData()
        {
            Title = retrieval["title"] as string ?? string.Empty,
            Artist = retrieval["artist"] as string ?? string.Empty,
            Album = retrieval["album"] as string ?? string.Empty,
            ArtworkUrl = retrieval["artwork"] as string ?? string.Empty
        };
    }

    public virtual string GetWindowUrl(WebDriver driver) => driver.Url;

    public virtual bool GetPauseState(WebDriver driver)
    {
        const string pauseScript = """
                                       const videoElement = document.querySelector('video');
                                       if (videoElement) {
                                           return videoElement.paused;  // Check if the video is paused
                                       } else {
                                           return null;
                                       }
                                   """;

        var isPaused = (bool)driver.ExecuteScript(pauseScript);
        Console.WriteLine($"Paused: {isPaused}");
        return isPaused;
    }

    public virtual TimeInfo GetTimeInfo(WebDriver driver)
    {
        const string timeScript = """
                                  const videoElement = document.querySelector('video');
                                  if (videoElement) {
                                      return {
                                          currentTime: videoElement.currentTime,
                                          duration: videoElement.duration,
                                          remainingTime: videoElement.duration - videoElement.currentTime
                                      };
                                  } else {
                                      return null;
                                  }
                                  """;

        var mediaInfo = (Dictionary<string, object>)driver.ExecuteScript(timeScript);

        // Declaration + default values in case parsing fails.
        double currentTime = 1;
        double durationTime = 300;
        double remainingTime = durationTime - currentTime;

        if (mediaInfo is not null)
        {
            try
            {
                currentTime = double.Parse(mediaInfo["currentTime"].ToString() ?? string.Empty);
                durationTime = double.Parse(mediaInfo["duration"].ToString() ?? string.Empty);
                remainingTime = double.Parse(mediaInfo["remainingTime"].ToString() ?? string.Empty);
            }
            catch
            {
                Console.WriteLine("Couldn't retrieve time. Are you on YTM?");
            }
        }
        else
        {
            Console.Out.WriteLine($"Couldn't retrieve {nameof(TimeInfo)}");
        }

        Console.WriteLine($"Current Time: {currentTime} seconds");
        Console.WriteLine($"Total Duration: {durationTime} seconds");
        Console.WriteLine($"Remaining Time: {remainingTime} seconds");

        return new TimeInfo(currentTime, durationTime, remainingTime);
    }

    public virtual string GetSongUrl(WebDriver driver)
    {
        // TODO: Implement!
        return Url;
    }
}