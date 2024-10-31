using OpenQA.Selenium;
using System.Text.RegularExpressions;
using OpenQA.Selenium.DevTools.V127.Page;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal partial class YtmRetriever : BaseRetriever
{
    public override string Name => "Youtube Music";
    public override string Url => "https://music.youtube.com/";

    public override string PlayingIconKey =>
        "https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/youtube-music-icon.png";

    public override string PausedIconKey => "ytm_paused";

    public override bool GetPauseState(WebDriver driver)
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

    public override TimeInfo? GetTimeInfo(WebDriver driver)
    {
        // There seems to be a bug where the UI disappears. This is very rare and has only happened a couple of times, but to ensure we retrieve a time this is a better system.
        // The reason why we prefer the UI over retrieving directly from the audio is because audio timestamps can stack on albums or autoplay.
        if (TryGetTimeInfoFromUi(driver, out var info1)) return info1;
        if (TryGetTimeInfoFromAudio(driver, out var info2)) return info2;

        // If we can't retrieve by any means, we return null.
        Console.Out.WriteLine($"Couldn't retrieve {nameof(TimeInfo)}");
        return null;
    }

    private bool TryGetTimeInfoFromUi(WebDriver driver, out TimeInfo? timeInfo)
    {
        var timeInfoElement = driver.FindElement(By.CssSelector("span.time-info.style-scope.ytmusic-player-bar")); // This element only contains the text "m:ss / m:ss".
        
        if (timeInfoElement is null)
        {
            timeInfo = null;
            return false;
        }

        string timeInfoText = timeInfoElement.Text; // eg "2:36 / 3:20".

        if (!YtmTimeRegex().IsMatch(timeInfoText))
        {
            Console.Out.WriteLine("Time string from ui element is of an invalid format! Can not parse.");
            timeInfo = null;
            return false;
        }

        var match = YtmTimeRegex().Match(timeInfoText);
        double currentTime = ConvertToSeconds(match.Groups[1].Value);
        double durationTime = ConvertToSeconds(match.Groups[2].Value);

        Console.Out.WriteLine("Found time from UI.");
        Console.Out.WriteLine($"CurrentTime: {currentTime}");
        Console.Out.WriteLine($"durationTime: {durationTime}");
        Console.Out.WriteLine($"remainingTime: {durationTime - currentTime}");

        timeInfo = new TimeInfo(currentTime, durationTime, durationTime - currentTime);
        return true;
    }

    /// <summary>
    /// Helper method to convert time strings (2:30, 1:53 etc.) to seconds.
    /// </summary>
    /// <param name="time">String in <see cref="YtmTimeRegex"/> format.</param>
    /// <returns><paramref name="time"/> in seconds.</returns>
    private static double ConvertToSeconds(string time)
    {
        var parts = time.Split(':');
        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        return minutes * 60 + seconds;
    }

    // Generated regex.
    [GeneratedRegex(@"(\d+:\d+) / (\d+:\d+)", RegexOptions.CultureInvariant, 1000)]
    private static partial Regex YtmTimeRegex();

    private bool TryGetTimeInfoFromAudio(WebDriver driver, out TimeInfo? timeInfo)
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

        if (mediaInfo is not null)
        {
            try
            {
                var currentTime = double.Parse(mediaInfo["currentTime"].ToString() ?? string.Empty);
                var durationTime = double.Parse(mediaInfo["duration"].ToString() ?? string.Empty);
                var remainingTime = double.Parse(mediaInfo["remainingTime"].ToString() ?? string.Empty);

                Console.Out.WriteLine("Found time from audio.");
                Console.Out.WriteLine($"CurrentTime: {currentTime}");
                Console.Out.WriteLine($"durationTime: {durationTime}");
                Console.Out.WriteLine($"remainingTime: {remainingTime}");

                timeInfo = new TimeInfo(currentTime, durationTime, remainingTime);
                return true;
            }
            catch
            {
                // Ignored.
            }
        }

        Console.Out.WriteLine("Could not find time from audio.");

        timeInfo = null;
        return false;
    }
}