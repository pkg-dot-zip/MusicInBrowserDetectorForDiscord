namespace YoutubeMusicDiscordRichPresenceCSharp.Services;

public interface IServiceResource
{
    public string Name { get; }
    public string Url { get; }
    public string PlayingIconKey { get; }
    public string PausedIconKey { get; }
}