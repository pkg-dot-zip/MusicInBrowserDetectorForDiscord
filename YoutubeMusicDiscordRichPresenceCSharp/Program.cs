using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal static class Program
{
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.AddServices().BuildServiceProvider();

        var program = serviceProvider.GetRequiredService<MusicBrowserDetectorProgram>();
        program.Run();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Main Program.
        services.AddSingleton<MusicBrowserDetectorProgram>();

        // Logger.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });

        // Util.
        services.AddSingleton<IRpcHandler, RpcHandler>();
        services.AddSingleton<IPresenceUpdater, PresenceUpdater>();
        services.AddSingleton<ISongPresenceHandler, SongPresenceHandler>();
        services.AddSingleton<IBrowserRetriever, BrowserRetriever>();
        return services;
    }
}