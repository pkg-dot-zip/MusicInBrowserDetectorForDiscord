using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

public abstract class BaseBrowserHandler : IBrowser
{
    protected WebDriver? Driver;
    protected BaseRetriever? Retriever;

    public abstract void OpenWindow(int port = IBrowser.DefaultPort);

    public abstract WebDriver GetDriver(int port = IBrowser.DefaultPort);

    public virtual bool IsRunning(int port = IBrowser.DefaultPort) => IsRunningAsync().Result;

    public virtual Task<bool> IsRunningAsync(int port = IBrowser.DefaultPort)
    {
        throw new NotImplementedException();
    }

    // <inheritdoc>
    public virtual void Close(int port = IBrowser.DefaultPort) => Driver?.Quit();

    public virtual BaseRetriever? GetRetriever() => Retriever;
}