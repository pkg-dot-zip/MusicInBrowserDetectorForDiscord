using OpenQA.Selenium;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public class SongRetriever
{
    public static CurrentPlayingInfo? FromBrowser(IWebDriver driver) => FromBrowser((IJavaScriptExecutor)driver);
    public static CurrentPlayingInfo? FromBrowser(IJavaScriptExecutor driver)
    {
        const string metadataScript = """
                                          return navigator.mediaSession.metadata ? {
                                              title: navigator.mediaSession.metadata.title,
                                              artist: navigator.mediaSession.metadata.artist,
                                              album: navigator.mediaSession.metadata.album,
                                              artwork: navigator.mediaSession.metadata.artwork[0].src
                                          } : null;
                              """;

        var metadata = (Dictionary<string, object>)driver.ExecuteScript(metadataScript);

        if (metadata is not null)
        {
            Console.WriteLine($"Title: {metadata["title"]}");
            Console.WriteLine($"Artist: {metadata["artist"]}");
            Console.WriteLine($"Album: {metadata["album"]}");

            // Set base information.
            var playingInfo = new CurrentPlayingInfo()
            {
                Title = metadata["title"] as string ?? string.Empty, // TODO: Throw instead!
                Artist = metadata["artist"] as string ?? string.Empty,
                Album = metadata["album"] as string ?? string.Empty,
                ArtworkUrl = metadata["artwork"] as string ?? string.Empty
            };

            // Set the time information.
            var (currentTime, durationTime, remainingTime) = GetTimeInfo(driver);
            playingInfo.CurrentTime = currentTime;
            playingInfo.DurationTime = durationTime;
            playingInfo.RemainingTime = remainingTime;

            // Set the url.
            playingInfo.SongUrl = GetSongUrl(driver);

            return playingInfo;
        }

        Console.WriteLine("No song is currently playing or metadata is unavailable.");
        return null;
    }


    private static (double, double, double) GetTimeInfo(IJavaScriptExecutor driver)
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

        Console.WriteLine($"Current Time: {mediaInfo["currentTime"]} seconds");
        Console.WriteLine($"Total Duration: {mediaInfo["duration"]} seconds");
        Console.WriteLine($"Remaining Time: {mediaInfo["remainingTime"]} seconds");

        var currentTime = double.Parse(mediaInfo["currentTime"].ToString()); // TODO: Throw if invalid!
        var durationTime = double.Parse(mediaInfo["duration"].ToString());
        var remainingTime = double.Parse(mediaInfo["remainingTime"].ToString());

        return (currentTime, durationTime, remainingTime);
    }

    private static string GetSongUrl(IJavaScriptExecutor driver)
    {
        // TODO: Implement!
        return "https://music.youtube.com/";
    }
}