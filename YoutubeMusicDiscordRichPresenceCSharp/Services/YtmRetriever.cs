namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

internal class YtmRetriever : BaseRetriever
{
    public override string Name => "Youtube Music";
    public override string Url => "https://music.youtube.com/";

    public override string PlayingIconKey => "https://uxwing.com/wp-content/themes/uxwing/download/brands-and-social-media/youtube-music-icon.png";
    public override string PausedIconKey => "https://cdn-icons-png.flaticon.com/512/4181/4181163.png";
}