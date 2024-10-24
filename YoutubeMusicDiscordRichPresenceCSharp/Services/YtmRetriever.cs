using OpenQA.Selenium;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal class YtmRetriever : BaseRetriever
{
    public override string Name => "Youtube Music";
    public override string Url => "https://music.youtube.com/";

    public override string PlayingIconKey => "https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/youtube-music-icon.png";
    public override string PausedIconKey => "https://cdn-icons-png.flaticon.com/512/4181/4181163.png";
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

                Console.Out.WriteLine($"CurrentTime: {currentTime}");
                Console.Out.WriteLine($"durationTime: {durationTime}");
                Console.Out.WriteLine($"remainingTime: {remainingTime}");

                return new TimeInfo(currentTime, durationTime, remainingTime);
            }
            catch
            {
                // ignored.
            }
        }

        Console.Out.WriteLine($"Couldn't retrieve {nameof(TimeInfo)}");
        return null;
    }
}