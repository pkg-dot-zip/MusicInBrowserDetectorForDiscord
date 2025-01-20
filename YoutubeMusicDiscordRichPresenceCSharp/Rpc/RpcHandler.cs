using DiscordRPC;
using DiscordRPC.Logging;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public class RpcHandler : IDisposable, IRpcHandler
{
    private const string ApplicationId = "1297469080273420329";
    private readonly DiscordRpcClient _client = new(ApplicationId);

    public void Initialize()
    {
        _client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

        _client.OnReady += (_, e) => { Console.WriteLine("Received Ready from user {0}", e.User.Username); };

        _client.OnPresenceUpdate += (_, e) => { Console.WriteLine("Received Update! {0}", e.Presence); };

        _client.Initialize(); // Connect to the RPC.
    }

    public void SetPresence(RichPresence presence)
    {
        _client.SetPresence(presence);
    }

    public void DeInitialize()
    {
        Dispose();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}