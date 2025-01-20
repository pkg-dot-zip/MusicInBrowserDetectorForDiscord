using DiscordRPC;
using DiscordRPC.Logging;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public static class RpcHandler
{
    private static readonly DiscordRpcClient Client = new("1297469080273420329");

    public static void Initialize()
    {
        Client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

        Client.OnReady += (_, e) => { Console.WriteLine("Received Ready from user {0}", e.User.Username); };

        Client.OnPresenceUpdate += (_, e) => { Console.WriteLine("Received Update! {0}", e.Presence); };

        Client.Initialize(); //Connect to the RPC.
    }

    public static void SetPresence(RichPresence presence)
    {
        Client.SetPresence(presence);
    }

    public static void Deinitialize()
    {
        Client.Dispose();
    }
}