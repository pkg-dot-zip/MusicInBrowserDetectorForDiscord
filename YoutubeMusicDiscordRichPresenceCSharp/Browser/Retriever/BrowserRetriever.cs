using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;

public class BrowserRetriever(ILogger<BrowserRetriever> logger) : IBrowserRetriever
{
    // Grabs the correct BrowserHandler for the current running 
    public IBrowser GetBrowserHandler()
    {
        var processlist = Process.GetProcesses();

        logger.LogInformation("Looking for Browser:");

        foreach (var process in processlist)
        {
            if (string.IsNullOrEmpty(process.MainWindowTitle)) continue; // Needs to have an open window.

            logger.LogInformation("Process: {name} ID: {id} Window title: {title}", process.ProcessName, process.Id, process.MainWindowTitle);

            if (process.ProcessName == "chrome")
            {
                logger.LogInformation("Found Chrome!");
                return new ChromeHandler();
            }

            if (process.ProcessName == "brave")
            {
                logger.LogInformation("Found Brave!");
                return new ChromeHandler();
            }
        }

        throw new InvalidOperationException("Couldn't find an opened browser! Check our github repository for information on what browsers are supported.");
    }
}