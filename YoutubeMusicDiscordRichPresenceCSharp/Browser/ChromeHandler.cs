using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace YoutubeMusicDiscordRichPresenceCSharp.Browser;

internal static class ChromeHandler
{
    private const int DefaultPort = 9222;

    // TODO: Pass path in params.
    // TODO: Throw if not valid here.
    public static void OpenChromeBrowserWithRemoteDebugging(int port = DefaultPort)
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

    public static IWebDriver GetChromeInstance(int port = DefaultPort)
    {
        return new ChromeDriver(new ChromeOptions
        {
            DebuggerAddress = $"localhost:{port}"
        });
    }

    public static async Task<bool> IsChromeRunningWithRemoteDebugging(int port = DefaultPort)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://localhost:{port}/json");
                return response.IsSuccessStatusCode;
            }
        }
        catch (HttpRequestException) // If an exception occurs (e.g., connection failure), Chrome is not running on the specified port.
        {
            return false;
        }
    }
}