using OpenQA.Selenium;
using YoutubeMusicDiscordRichPresenceCSharp.Browser;

namespace YoutubeMusicDiscordRichPresenceCSharp.Rpc;

public class PresenceUpdater(IRpcHandler rpcHandler) : IPresenceUpdater
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
                rpcHandler.SetPresence(SongPresenceHandler.GetSongPresence(retriever, playingInfo));
            }
            else
            {
                Console.WriteLine("Couldn't find song, thus no rpc set.");
            }

            return true;
        }
        catch (NoSuchWindowException ex)
        {
            Console.Out.WriteLine("Looks like the browser window we were hooked too was closed. Disconnected.");
            return false;
        }
    }
}