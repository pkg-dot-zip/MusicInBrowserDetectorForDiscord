using OpenQA.Selenium;

namespace YoutubeMusicDiscordRichPresenceCSharp.Models;

public class CurrentPlayingInfo
{
    public string Artist { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string ArtworkUrl { get; set; } = string.Empty;

    public static CurrentPlayingInfo? FromBrowser(IWebDriver driver) => FromBrowser((IJavaScriptExecutor)driver);
    public static CurrentPlayingInfo? FromBrowser(IJavaScriptExecutor driver)
    {
        const string script = """
                                          return navigator.mediaSession.metadata ? {
                                              title: navigator.mediaSession.metadata.title,
                                              artist: navigator.mediaSession.metadata.artist,
                                              album: navigator.mediaSession.metadata.album,
                                              artwork: navigator.mediaSession.metadata.artwork[0].src
                                          } : null;
                              """;

        var metadata = (Dictionary<string, object>)driver.ExecuteScript(script);

        if (metadata is not null)
        {
            Console.WriteLine($"Title: {metadata["title"]}");
            Console.WriteLine($"Artist: {metadata["artist"]}");
            Console.WriteLine($"Album: {metadata["album"]}");
            return new CurrentPlayingInfo()
            {
                Title = metadata["title"] as string ?? string.Empty,
                Artist = metadata["artist"] as string ?? string.Empty,
                Album = metadata["album"] as string ?? string.Empty,
                ArtworkUrl = metadata["artwork"] as string ?? string.Empty
            };
        }

        Console.WriteLine("No song is currently playing or metadata is unavailable.");
        return null;
    }
}