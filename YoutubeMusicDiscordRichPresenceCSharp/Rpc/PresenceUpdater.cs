using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public class PresenceUpdater(ILogger<PresenceUpdater> logger, IRpcHandler rpcHandler, ISongPresenceHandler songPresenceHandler) : IPresenceUpdater
{
    public bool UpdatePresence(IBrowser browserHandler)
    {
        try
        {
            var retriever = browserHandler.GetRetriever();
            if (retriever is null) return false;


            var playingInfo = retriever.FromBrowser(browserHandler);

            if (playingInfo is not null)
            {
                rpcHandler.SetPresence(songPresenceHandler.GetSongPresence(retriever, playingInfo));
            }
            else
            {
                logger.LogInformation("Couldn't find song, thus no rpc set.");
            }

            return true;
        }
        catch (NoSuchWindowException ex)
        {
            logger.LogWarning("Looks like the browser window we were hooked too was closed. Disconnected.");
            return false;
        }
    }
}