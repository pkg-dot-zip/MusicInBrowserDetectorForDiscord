using BrowserLib.Browser;
using YtmRcpLib.Models;

namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal interface IServiceRetriever
{
    public CurrentPlayingInfo? FromBrowser(IBrowser browser);
}