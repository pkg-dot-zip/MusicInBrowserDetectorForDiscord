using BrowserLib.Browser;
using YtmRcpLib.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    private static readonly IBrowser BrowserHandler = new ChromeHandler();

    public static void Main(string[] args)
    {
        Run(BrowserHandler);
    }

    private static void Run(IBrowser browserHandler, int refreshInterval = 20000)
    {
        Initialize();

        if (!browserHandler.IsRunning()) browserHandler.OpenWindow();

        Thread.Sleep(2000); // Wait 2 sec.

        while (true)
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

        Deinitialize();
    }

    private static void Initialize()
    {
        RpcHandler.Initialize();
    }

    private static void Deinitialize()
    {
        RpcHandler.Deinitialize();
        BrowserHandler.Close();
    }
}