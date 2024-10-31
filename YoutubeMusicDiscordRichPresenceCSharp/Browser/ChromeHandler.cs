using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

/// <summary>
/// Browser Handler that supports most Chromium browser, such as Chrome and Brave.
/// </summary>
public class ChromeHandler : BaseBrowserHandler
{
    // string arguments = $@"--remote-debugging-port={port} --user-data-dir=""C:\ChromeDebug"""

    // <inheritdoc>
    public override WebDriver GetDriver(int port = IBrowser.DefaultPort)
    {
        if (Driver is not null) return Driver;

        Driver = new ChromeDriver(new ChromeOptions
        {
            DebuggerAddress = $"localhost:{port}"
        });

        // First time instead of just returning we also prepare the BaseRetriever.
        var windowHandles = Driver.WindowHandles; // Get all tabs or windows.

        // Get all subclasses of BaseRetriever using reflection.
        var retrieverTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseRetriever)) && !t.IsAbstract)
            .ToList();

        foreach (var windowHandle in windowHandles)
        {
            if (Retriever is not null) break; // Escape if already found a streaming service.
            Driver.SwitchTo().Window(windowHandle); // Switch to each window/tab. You can see this in your browser.

            foreach (var retrieverType in retrieverTypes)
            {
                var retrieverInstance = (BaseRetriever)Activator.CreateInstance(retrieverType);

                // Check if we are in a page we can handle, like ytm or sc.
                if (Driver.Url.StartsWith(retrieverInstance.Url))
                {
                    Retriever = retrieverInstance;
                    break;
                }
            }
        }

        return Driver;
    }
}