using System.Diagnostics;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

public class ChromeHandler : IBrowser
{
    private ChromeDriver? _driver;

    // TODO: Pass path in params.
    // TODO: Throw if not valid here.
    // <inheritdoc>
    public void OpenWindow(int port)
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
    public WebDriver GetDriver(int port)
    {
        if (_driver is not null) return _driver;

        _driver = new ChromeDriver(new ChromeOptions
        {
            DebuggerAddress = $"localhost:{port}"
        });

        //
        // var windowHandles = _driver.WindowHandles; // Get all tabs or windows.
        //
        // foreach (var windowHandle in windowHandles)
        // {
        //     _driver.SwitchTo().Window(windowHandle); // Switch to each window/tab.
        //
        //     // Skip if the current tab is not YouTube Music.
        //     if (!_driver.Url.Contains("music.youtube.com")) continue;
        //    
        //     Console.WriteLine("Switched to YouTube Music tab.");
        //     return _driver;
        // }

        return _driver;
    }

    // <inheritdoc>
    public bool IsRunning(int port) => IsRunningAsync(port).Result;

    // <inheritdoc>
    public async Task<bool> IsRunningAsync(int port)
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

    // <inheritdoc>
    public void Close(int port)
    {
        _driver?.Quit();
    }

    public BaseRetriever? GetRetriever(int port)
    {
        // TODO: This assumes there is only 1 tab open! Iterate through all tabs instead using: GetDriver().SwitchTo().Window(windowHandle);

        // Get all subclasses of BaseRetriever using reflection.
        var retrieverTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseRetriever)) && !t.IsAbstract)
            .ToList();

        foreach (var retrieverType in retrieverTypes)
        {
            var retrieverInstance = (BaseRetriever)Activator.CreateInstance(retrieverType);

            // Check if we are in a page we can handle, like ytm.
            if (GetDriver(port).Url.StartsWith(retrieverInstance.Url)) return retrieverInstance;
        }

        return null;
    }
}