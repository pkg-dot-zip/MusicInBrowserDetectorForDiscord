using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YoutubeMusicDiscordRichPresenceCSharp.Models;

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
            Console.Out.WriteLine("Not on {0} anymore!", Name);
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

        Console.WriteLine("Title: {0}", retrieval["title"]);
        Console.WriteLine("Artist: {0}", retrieval["artist"]);
        Console.WriteLine("Album: {0}", retrieval["album"]);

        return new MetaData()
        {
            Title = retrieval["title"] as string ?? string.Empty,
            Artist = retrieval["artist"] as string ?? string.Empty,
            Album = retrieval["album"] as string ?? string.Empty,
            ArtworkUrl = retrieval["artwork"] as string ?? string.Empty
        };
    }

    public virtual string GetWindowUrl(WebDriver driver) => driver.Url;

    public abstract bool GetPauseState(WebDriver driver);

    public abstract TimeInfo? GetTimeInfo(WebDriver driver);

    public virtual string GetSongUrl(WebDriver driver)
    {
        // TODO: Implement!
        return Url;
    }
}