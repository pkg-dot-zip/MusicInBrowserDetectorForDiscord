namespace YtmRcpLib.Models.Services;

public interface IService
{
    public string Name { get; }
    public string Url { get; }
    public string PlayingIconKey { get; }
    public string PausedIconKey { get; }

    public bool GetPauseState();
    public string GetSongUrl();
    public TimeInfo GetTimeInfo();
}