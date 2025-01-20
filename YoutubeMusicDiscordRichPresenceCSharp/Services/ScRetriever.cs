using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal class ScRetriever : BaseRetriever
{
    public override string Name => "Soundcloud";
    public override string Url => "https://soundcloud.com/";

    public override string PlayingIconKey => "https://img.freepik.com/premium-vector/soundcloud-logo_578229-231.jpg";
    public override string PausedIconKey => "https://cdn-icons-png.flaticon.com/512/190/190521.png";

    public override TimeInfo? GetTimeInfo(WebDriver driver)
    {
        const string timeScript = """
                                      const progressBar = document.querySelector('.playbackTimeline__progressWrapper[role="progressbar"]');
                                  
                                      if (progressBar) {
                                          const currentTime = parseFloat(progressBar.getAttribute('aria-valuenow'));  // Current time in seconds.
                                          const durationTime = parseFloat(progressBar.getAttribute('aria-valuemax'));  // Total duration in seconds.
                                          const remainingTime = durationTime - currentTime;  // Remaining time in seconds.
                                  
                                          return {
                                              currentTime: currentTime,
                                              durationTime: durationTime,
                                              remainingTime: remainingTime
                                          };
                                      } else {
                                          return null;  // Could not find the progress bar
                                      }
                                  """;

        var timeInfoDict = (Dictionary<string, object>)driver.ExecuteScript(timeScript);

        if (timeInfoDict is not null)
        {
            var timeInfo = new TimeInfo(
                Convert.ToDouble(timeInfoDict["currentTime"]),
                Convert.ToDouble(timeInfoDict["durationTime"]),
                Convert.ToDouble(timeInfoDict["remainingTime"])
            );

            Console.WriteLine("Current Time: {0} seconds", timeInfo.CurrentTime);
            Console.WriteLine("Duration: {0} seconds", timeInfo.DurationTime);
            Console.WriteLine("Remaining Time: {0} seconds", timeInfo.RemainingTime);

            return timeInfo;
        }

        Console.WriteLine("Could not retrieve time information.");
        return null;
    }

    public override bool GetPauseState(WebDriver driver)
    {
        const string pauseScript = """
                                       const playPauseButton = document.querySelector('.playControl.sc-ir.playControls__control.playControls__play');
                                   
                                       if (playPauseButton) {
                                           if (playPauseButton.classList.contains('playing')) {
                                               return false;  // Song is playing here!
                                           } else {
                                               return true;
                                           }
                                       } else {
                                           return null;   // Could not find the play/pause button.
                                       }
                                   """;

        var isPaused = driver.ExecuteScript(pauseScript);

        if (isPaused is null)
        {
            Console.Out.WriteLine("Couldn't retrieve {0}. Assuming false.", nameof(isPaused));
            return false;
        }

        Console.WriteLine("Paused: {0}", (bool)isPaused);
        return (bool)isPaused;
    }
}