using System.Timers;
using Microsoft.Extensions.Logging;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;
using Timer = System.Timers.Timer;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public class MusicBrowserDetectorProgram(
    ILogger<MusicBrowserDetectorProgram> logger,
    IBrowserRetriever browserRetriever,
    IPresenceUpdater presenceUpdater,
    IRpcHandler rpcHandler)
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
        logger.LogInformation("Starting Timer...");
        _timer.Start();
        logger.LogInformation("Started Timer.");
    }

    private void StopTimer()
    {
        logger.LogInformation("Stopping Timer...");
        _timer.Stop();
        logger.LogInformation("Stopped Timer.");
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        // TODO: Only do this once.
        _browserHandler.GetDriver(); // Call this so retriever gets initialized. Great system cough cough :/

        presenceUpdater.UpdatePresence(_browserHandler);
    }

    private void Initialize()
    {
        logger.LogInformation("Initializing...");
        rpcHandler.Initialize();
        logger.LogInformation("Initialized.");
        logger.LogInformation("\n\n\tWrite quit (or q) to exit.\t\t");

        _timer.Elapsed += OnTimerElapsed;
    }

    private void DeInitialize()
    {
        _timer.Elapsed -= OnTimerElapsed;
        logger.LogInformation("Deinitializing...");
        rpcHandler.DeInitialize();
        _browserHandler.Close();
        logger.LogInformation("Deinitialized.");
        Environment.Exit(0);
    }
}