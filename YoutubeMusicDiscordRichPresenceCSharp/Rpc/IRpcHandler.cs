using DiscordRPC;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public interface IRpcHandler
{
    void Initialize();
    void SetPresence(RichPresence presence);
    void DeInitialize();
}