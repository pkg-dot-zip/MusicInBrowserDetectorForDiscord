using System.Timers;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;
using Timer = System.Timers.Timer;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public class MusicBrowserDetectorProgram(IBrowserRetriever browserRetriever, IPresenceUpdater presenceUpdater, IRpcHandler rpcHandler)
{
    private bool _shouldQuit = false;
    private readonly IBrowser _browserHandler = browserRetriever.GetBrowserHandler();
    private readonly Timer _timer = new Timer
    {
        AutoReset = true,
        Enabled = false,
        Interval = 5000,
    };

    public void Run()
    {
        Initialize();

        Thread.Sleep(2000); // Wait 2 sec.

        StartTimer();
        while (!_shouldQuit)
        {
            var input = Console.ReadLine()?.ToLower().Trim();
            _shouldQuit = input is "quit" or "q";
            if (_shouldQuit) StopTimer();
        }

        DeInitialize();
    }

    private void StartTimer()
    {
        Console.WriteLine("Starting Timer...");
        _timer.Start();
        Console.WriteLine("Started Timer.");
    }

    private void StopTimer()
    {
        Console.WriteLine("Stopping Timer...");
        _timer.Stop();
        Console.WriteLine("Stopped Timer.");
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        // TODO: Only do this once.
        _browserHandler.GetDriver(); // Call this so retriever gets initialized. Great system cough cough :/

        presenceUpdater.UpdatePresence(_browserHandler);
    }

    private void Initialize()
    {
        Console.WriteLine("Initializing...");
        rpcHandler.Initialize();
        Console.WriteLine("Initialized.");
        Console.WriteLine("\n\n\tWrite quit (or q) to exit.\t\t");

        _timer.Elapsed += OnTimerElapsed;
    }

    private void DeInitialize()
    {
        _timer.Elapsed -= OnTimerElapsed;
        Console.WriteLine("Deinitializing...");
        rpcHandler.DeInitialize();
        _browserHandler.Close();
        Console.WriteLine("Deinitialized.");
        Environment.Exit(0);
    }
}