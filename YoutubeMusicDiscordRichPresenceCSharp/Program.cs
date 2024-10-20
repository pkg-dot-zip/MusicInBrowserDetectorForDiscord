using BrowserLib.Browser;
using YtmRcpLib.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    private static readonly IBrowser BrowserHandler = new ChromeHandler();
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
        while (!_shouldQuit)
        {
            var playingInfo = SongRetriever.FromBrowser(browserHandler.GetDriver());

            if (playingInfo is not null)
            {
                RpcHandler.SetPresence(SongPresenceHandler.GetSongPresence(playingInfo));
            }
            else
            {
                Console.WriteLine("Couldn't find song, thus no rpc set.");
            }

            Thread.Sleep(refreshInterval);
        }
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