using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    private static bool _shouldQuit = false;

    private static readonly IBrowserRetriever BrowserRetriever = new BrowserRetriever();

    public static void Main(string[] args) => Run(BrowserRetriever.GetBrowserHandler());

    /// <summary>
    /// Run the program.
    /// </summary>
    /// <param name="browserHandler">What browser should be used.</param>
    /// <param name="refreshInterval">How often rpc should be updated in ms.</param>
    private static void Run(IBrowser browserHandler, int refreshInterval = 20000)
    {
        Initialize();

        Thread.Sleep(2000); // Wait 2 sec.

        Thread presenceThread = new Thread(() => UpdatePresence(browserHandler, refreshInterval))
        {
            Name = "musicinbrowserrpcupdatethread",
            IsBackground = true,
        };
        presenceThread.Start();

        while (!_shouldQuit)
        {
            var input = Console.ReadLine()?.ToLower().Trim();
            _shouldQuit = input is "quit" or "q";
        }

        presenceThread.Join();
        Deinitialize(browserHandler);
    }

    private static void UpdatePresence(IBrowser browserHandler, int refreshInterval)
    {
        browserHandler.GetDriver(); // Call this so retriever gets initialized. Great system cough cough :/
        var retriever = browserHandler.GetRetriever();
        if (retriever is null)
        {
            Console.WriteLine("Couldn't find support for this service. Not updating.");
            return;
        }

        while (!_shouldQuit)
        {
            try
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
            catch (NoSuchWindowException ex)
            {
                Console.Out.WriteLine("Looks like the browser window we were hooked too was closed. Disconnected.");
                _shouldQuit = true;
                Deinitialize(browserHandler);
            }
        }
    }

    private static void Initialize()
    {
        Console.WriteLine("Initializing...");
        RpcHandler.Initialize();
        Console.WriteLine("Initialized.");
        Console.WriteLine("\n\n\tWrite quit (or q) to exit.\t\t");
    }

    private static void Deinitialize(IBrowser browserHandler)
    {
        Console.WriteLine("Deinitializing...");
        RpcHandler.Deinitialize();
        browserHandler.Close();
        Console.WriteLine("Deinitialized.");
        Environment.Exit(0);
    }
}