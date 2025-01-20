using DiscordRPC;
using DiscordRPC.Logging;
using Microsoft.Extensions.Logging;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public class RpcHandler(ILogger<RpcHandler> logger) : IDisposable, IRpcHandler
{
    private const string ApplicationId = "1297469080273420329";
    private readonly DiscordRpcClient _client = new(ApplicationId);

    public void Initialize()
    {
        _client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

        _client.OnReady += (_, e) => { logger.LogInformation("Received Ready from user {username}", e.User.Username); };

        _client.OnPresenceUpdate += (_, e) => { logger.LogInformation("Received Update! {presence}", e.Presence); };

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