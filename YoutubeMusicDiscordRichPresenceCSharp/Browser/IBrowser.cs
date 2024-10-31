using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

public interface IBrowser
{
    public const int DefaultPort = 9222;

    /// <summary>
    /// Returns the <see cref="WebDriver"/> to use for this browser.
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public WebDriver GetDriver(int port = DefaultPort);

    public void Close(int port = DefaultPort);

    public BaseRetriever? GetRetriever();
}