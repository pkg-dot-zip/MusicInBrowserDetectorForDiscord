using OpenQA.Selenium;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public class SongRetriever
{
    public static CurrentPlayingInfo? FromBrowser(IWebDriver driver) => FromBrowser((IJavaScriptExecutor)driver);

    private static CurrentPlayingInfo? FromBrowser(IJavaScriptExecutor driver)
    {
        if (!GetWindowUrl(driver).Contains("music.youtube")) return null;

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

    private static MetaData GetMetaData(IJavaScriptExecutor driver)
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

    private static string GetWindowUrl(IJavaScriptExecutor driver)
    {
        var url = (string)driver.ExecuteScript("return window.location.href;");
        return url ?? string.Empty;
    }

    private static bool GetPauseState(IJavaScriptExecutor driver)
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

    private static TimeInfo GetTimeInfo(IJavaScriptExecutor driver)
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

        Console.WriteLine($"Current Time: {currentTime} seconds");
        Console.WriteLine($"Total Duration: {durationTime} seconds");
        Console.WriteLine($"Remaining Time: {remainingTime} seconds");

        return new TimeInfo(currentTime, durationTime, remainingTime);
    }

    private static string GetSongUrl(IJavaScriptExecutor driver)
    {
        // TODO: Implement!
        return "https://music.youtube.com/";
    }
}