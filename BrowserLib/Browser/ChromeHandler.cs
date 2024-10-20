using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BrowserLib.Browser;

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
    public IWebDriver GetDriver(int port)
    {
        return _driver ??= new ChromeDriver(new ChromeOptions
        {
            DebuggerAddress = $"localhost:{port}"
        });
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
}