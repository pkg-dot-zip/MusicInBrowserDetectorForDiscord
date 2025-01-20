using YoutubeMusicDiscordRichPresenceCSharp.Browser;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public interface IPresenceUpdater
{
    bool UpdatePresence(IBrowser browserHandler);
}