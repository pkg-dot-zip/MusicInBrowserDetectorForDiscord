using OpenQA.Selenium;

namespace BrowserLib.Browser;

public interface IBrowser
{
    private const int DefaultPort = 9222;

    /// <summary>
    /// Opens a window of the browser with the <i>--remote-debugging-port</i> parameter.
    /// </summary>
    /// <param name="port">Port to use for the <i>--remote-debugging-port</i> parameter.</param>
    public void OpenWindow(int port = DefaultPort);

    /// <summary>
    /// Returns the <see cref="WebDriver"/> to use for this browser.
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public WebDriver GetDriver(int port = DefaultPort);

    /// <summary>
    /// Checks if the browser is already running and readable.
    /// </summary>
    /// <param name="port"></param>
    /// <returns><c>true</c> if the browser was found running and is readable. <c>false</c> if not.</returns>
    public bool IsRunning(int port = DefaultPort);

    /// <summary>
    /// Asynchronously checks if the browser is already running and readable.
    /// </summary>
    /// <param name="port"></param>
    /// <returns><c>true</c> if the browser was found running and is readable. <c>false</c> if not.</returns>
    public Task<bool> IsRunningAsync(int port = DefaultPort);

    public void Close(int port = DefaultPort);
}