using OpenQA.Selenium;

namespace YoutubeMusicDiscordRichPresenceCSharp.Models;

public class CurrentPlayingInfo
{
    public string Artist { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string ArtworkUrl { get; set; } = string.Empty;
    public string SongUrl { get; set; } = string.Empty;
    public double CurrentTime { get; set; } // In seconds.
    public double DurationTime { get; set; } // In seconds.
    public double RemainingTime { get; set; } // In seconds.

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

            var playingInfo = new CurrentPlayingInfo()
            {
                Title = metadata["title"] as string ?? string.Empty, // TODO: Throw instead!
                Artist = metadata["artist"] as string ?? string.Empty,
                Album = metadata["album"] as string ?? string.Empty,
                ArtworkUrl = metadata["artwork"] as string ?? string.Empty
            };

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

            if (mediaInfo is not null)
            {
                Console.WriteLine($"Current Time: {mediaInfo["currentTime"]} seconds");
                Console.WriteLine($"Total Duration: {mediaInfo["duration"]} seconds");
                Console.WriteLine($"Remaining Time: {mediaInfo["remainingTime"]} seconds");

                playingInfo.CurrentTime = double.Parse(mediaInfo["currentTime"].ToString()); // TODO: Throw if invalid!
                playingInfo.DurationTime = double.Parse(mediaInfo["duration"].ToString());
                playingInfo.RemainingTime = double.Parse(mediaInfo["remainingTime"].ToString());
            }
            else
            {
                Console.WriteLine("Couldn't find time info.");
            }

            const string urlScript = """
                                     const videoElement = document.querySelector('video');
                                     if (videoElement) {
                                         const videoId = videoElement.baseURI.split("v=")[1]?.split("&")[0];  // Extract the videoId in this convoluted way :/
                                         if (videoId) {
                                             return `https://music.youtube.com/watch?v=${videoId}`;
                                         }
                                     }
                                     return null;
                                     """;

            var urlInfo = (string)driver.ExecuteScript(urlScript);

            if (urlInfo is not null)
            {
                playingInfo.SongUrl = urlInfo; // TODO: Throw instead!

                Console.WriteLine($"Song url: {playingInfo.SongUrl}");
            }
            else
            {
                Console.WriteLine("Couldn't find song url.");
            }

            return playingInfo;
        }

        Console.WriteLine("No song is currently playing or metadata is unavailable.");
        return null;
    }
}