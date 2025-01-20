using Microsoft.Extensions.DependencyInjection;
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

        // Util.
        services.AddSingleton<IRpcHandler, RpcHandler>();
        services.AddSingleton<IPresenceUpdater, PresenceUpdater>();
        services.AddSingleton<ISongPresenceHandler, SongPresenceHandler>();
        services.AddSingleton<IBrowserRetriever, BrowserRetriever>();
        return services;
    }
}