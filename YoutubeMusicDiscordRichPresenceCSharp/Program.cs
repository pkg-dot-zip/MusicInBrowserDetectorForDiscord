using YoutubeMusicDiscordRichPresenceCSharp.Browser.Retriever;

namespace YoutubeMusicDiscordRichPresenceCSharp;

internal class Program
{
    public static void Main(string[] args)
    {
        var program = new MusicBrowserDetectorProgram(new BrowserRetriever());
        program.Run();
    }
}