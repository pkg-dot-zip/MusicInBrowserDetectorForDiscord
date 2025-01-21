using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using Microsoft.Extensions.Logging;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public class RpcHandler(ILogger<RpcHandler> logger) : IDisposable, IRpcHandler
{
    private const string ApplicationId = "1297469080273420329";
    private readonly DiscordRpcClient _client = new(ApplicationId);

    public void Initialize()
    {
        _client.Logger = new ConsoleLogger { Level = LogLevel.Warning };

        _client.OnReady += OnReady;
        _client.OnPresenceUpdate += OnPresenceUpdate;

        _client.Initialize(); // Connect to the RPC.
    }

    private void OnReady(object sender, ReadyMessage e)
    {
        logger.LogInformation("Received Ready from user {username}", e.User.Username);
    }

    private void OnPresenceUpdate(object sender, PresenceMessage e)
    {
        logger.LogInformation("Received Update! {presence}", e.Presence);
    }

    public void SetPresence(RichPresence presence)
    {
        _client.SetPresence(presence);
    }

    public void DeInitialize()
    {
        _client.OnReady -= OnReady;
        _client.OnPresenceUpdate -= OnPresenceUpdate;
        Dispose();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}