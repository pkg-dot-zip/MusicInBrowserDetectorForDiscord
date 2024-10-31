using System.Diagnostics;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YtmRcpLib.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    private static readonly IBrowser BrowserHandler = GetBrowserHandler();
    private static bool _shouldQuit = false;

    public static void Main(string[] args)
    {
        Run(BrowserHandler);
    }

    /// <summary>
    /// Run the program.
    /// </summary>
    /// <param name="browserHandler">What browser should be used.</param>
    /// <param name="refreshInterval">How often rpc should be updated in ms.</param>
    private static void Run(IBrowser browserHandler, int refreshInterval = 20000)
    {
        Initialize();

        if (!browserHandler.IsRunning()) browserHandler.OpenWindow();

        Thread.Sleep(2000); // Wait 2 sec.

        Thread presenceThread = new Thread(() => UpdatePresence(browserHandler, refreshInterval))
        {
            Name = "ytmrpcupdatethread",
            IsBackground = true,
        };
        presenceThread.Start();

        while (!_shouldQuit)
        {
            var input = Console.ReadLine()?.ToLower().Trim();
            _shouldQuit = input is "quit" or "q";
        }

        presenceThread.Join();
        Deinitialize();
    }

    private static void UpdatePresence(IBrowser browserHandler, int refreshInterval)
    {
        BrowserHandler.GetDriver(); // Call this so retriever gets initialized. Great system cough cough :/
        var retriever = BrowserHandler.GetRetriever();
        if (retriever is null)
        {
            Console.WriteLine("Couldn't find support service. Not updating.");
            return;
        }

        while (!_shouldQuit)
        {
            var playingInfo = retriever.FromBrowser(browserHandler);

            if (playingInfo is not null)
            {
                RpcHandler.SetPresence(SongPresenceHandler.GetSongPresence(retriever, playingInfo));
            }
            else
            {
                Console.WriteLine("Couldn't find song, thus no rpc set.");
            }

            Thread.Sleep(refreshInterval);
        }
    }

    private static IBrowser GetBrowserHandler()
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
        }

        throw new InvalidOperationException("Couldn't find an opened browser! Check our github repository for information on what browsers are supported.");
    }

    private static void Initialize()
    {
        Console.WriteLine("Initializing...");
        RpcHandler.Initialize();
        Console.WriteLine("Initialized.");
        Console.WriteLine("\n\n\tWrite quit (or q) to exit.\t\t");
    }

    private static void Deinitialize()
    {
        Console.WriteLine("Deinitializing...");
        RpcHandler.Deinitialize();
        BrowserHandler.Close();
        Console.WriteLine("Deinitialized.");
    }
}