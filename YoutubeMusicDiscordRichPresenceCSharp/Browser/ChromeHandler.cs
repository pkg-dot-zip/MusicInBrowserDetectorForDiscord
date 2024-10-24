using System.Diagnostics;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

public class ChromeHandler : BaseBrowserHandler
{
    // TODO: Pass path in params.
    // TODO: Throw if not valid here.
    // <inheritdoc>
    public override void OpenWindow(int port = IBrowser.DefaultPort)
    {
        string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"; // Default path.

        // Arguments for remote debugging.
        string arguments = $@"--remote-debugging-port={port} --user-data-dir=""C:\ChromeDebug""";

        Process.Start(new ProcessStartInfo
        {
            FileName = chromePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        });

        Console.WriteLine($"Chrome started with remote debugging on port {port}.");
    }

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

    // <inheritdoc>
    public override async Task<bool> IsRunningAsync(int port = IBrowser.DefaultPort)
    {
        try
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{port}/json");
                return response.IsSuccessStatusCode;
            }
        }
        // If an exception occurs (e.g., connection failure), Chrome is not running on the specified port.
        catch (HttpRequestException)
        {
            return false;
        }
    }
}