namespace YtmRcpLib.Models;

public class CurrentPlayingInfo
{
    public MetaData? MetaData { get; set; }
    public string SongUrl { get; set; } = string.Empty;
    public TimeInfo? TimeInfo { get; set; }

    public bool IsPaused { get; set; }

    public bool IsNothing() => MetaData is null;
}