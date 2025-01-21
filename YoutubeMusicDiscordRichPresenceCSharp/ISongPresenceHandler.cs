using DiscordRPC;
using YoutubeMusicDiscordRichPresenceCSharp.Models;
using YoutubeMusicDiscordRichPresenceCSharp.Services;

namespace YoutubeMusicDiscordRichPresenceCSharp;

public interface ISongPresenceHandler
{
    RichPresence GetSongPresence(IServiceResource resource, CurrentPlayingInfo info);
}