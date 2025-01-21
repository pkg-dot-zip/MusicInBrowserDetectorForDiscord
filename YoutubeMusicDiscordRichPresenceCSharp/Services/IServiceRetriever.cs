using YoutubeMusicDiscordRichPresenceCSharp.Browser;
using YoutubeMusicDiscordRichPresenceCSharp.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal interface IServiceRetriever
{
    public CurrentPlayingInfo? FromBrowser(IBrowser browser);
}