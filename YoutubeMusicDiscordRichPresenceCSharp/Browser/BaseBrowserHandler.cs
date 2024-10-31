using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

public abstract class BaseBrowserHandler : IBrowser
{
    protected WebDriver? Driver;
    protected BaseRetriever? Retriever;

    public abstract WebDriver GetDriver(int port = IBrowser.DefaultPort);

    // <inheritdoc>
    public virtual void Close(int port = IBrowser.DefaultPort) => Driver?.Quit();

    public virtual BaseRetriever? GetRetriever() => Retriever;
}