namespace YtmRcpLib.Models;

public class CurrentPlayingInfo
{
    public string Artist { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string ArtworkUrl { get; set; } = string.Empty;
    public string SongUrl { get; set; } = string.Empty;
    public double CurrentTime { get; set; } // In seconds.
    public double DurationTime { get; set; } // In seconds.
    public double RemainingTime { get; set; } // In seconds.

    public bool IsPaused { get; set; }
}