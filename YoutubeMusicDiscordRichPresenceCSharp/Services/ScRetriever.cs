using OpenQA.Selenium;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal class ScRetriever : BaseRetriever
{
    public override string Name => "Soundcloud";
    public override string Url => "https://soundcloud.com/";

    public override string PlayingIconKey => "https://img.freepik.com/premium-vector/soundcloud-logo_578229-231.jpg";
    public override string PausedIconKey => "https://cdn-icons-png.flaticon.com/512/190/190521.png";


    public override bool GetPauseState(WebDriver driver)
    {
        const string pauseScript = """
                                       const audioElement = document.querySelector('audio');
                                       if (audioElement) {
                                           return audioElement.paused;  
                                       } else {
                                           return null;
                                       }
                                   """;

        var isPaused = driver.ExecuteScript(pauseScript);

        if (isPaused is null)
        {
            Console.Out.WriteLine($"Couldn't retrieve {nameof(isPaused)}. Assuming false.");
            return false;
        }

        Console.WriteLine($"Paused: {(bool)isPaused}");
        return (bool)isPaused;
    }
}