using YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;
using YoutubeMusicDiscordRichPresenceCSharp.Rpc;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    public static void Main(string[] args)
    {
        var rpcHandler = new RpcHandler();
        var program = new MusicBrowserDetectorProgram(new BrowserRetriever(), new PresenceUpdater(rpcHandler), rpcHandler);
        program.Run();
    }
}