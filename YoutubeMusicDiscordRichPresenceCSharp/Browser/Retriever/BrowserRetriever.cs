using System.Diagnostics;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;

public class BrowserRetriever : IBrowserRetriever
{
    // Grabs the correct BrowserHandler for the current running 
    public IBrowser GetBrowserHandler()
    {
        var processlist = Process.GetProcesses();

        Console.Out.WriteLine("Looking for Browser:");

        foreach (var process in processlist)
        {
            if (string.IsNullOrEmpty(process.MainWindowTitle)) continue; // Needs to have an open window.

            Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);

            if (process.ProcessName == "chrome")
            {
                Console.Out.WriteLine("Found Chrome!");
                return new ChromeHandler();
            }

            if (process.ProcessName == "brave")
            {
                Console.Out.WriteLine("Found Brave!");
                return new ChromeHandler();
            }
        }

        throw new InvalidOperationException("Couldn't find an opened browser! Check our github repository for information on what browsers are supported.");
    }
}