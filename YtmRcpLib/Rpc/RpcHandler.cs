using DiscordRPC;
using DiscordRPC.Logging;

namespace YtmRcpLib.Rpc;

public static class RpcHandler
{
    private static readonly DiscordRpcClient Client = new DiscordRpcClient("1297469080273420329");

    public static void Initialize()
    {
        //Set the logger
        Client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

        //Subscribe to events
        Client.OnReady += (sender, e) =>
        {
            Console.WriteLine("Received Ready from user {0}", e.User.Username);
        };

        Client.OnPresenceUpdate += (sender, e) =>
        {
            Console.WriteLine("Received Update! {0}", e.Presence);
        };

        //Connect to the RPC
        Client.Initialize();
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