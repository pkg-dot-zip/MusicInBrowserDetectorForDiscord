namespace YtmRcpLib.Models;

public class TimeInfo(double currentTime, double durationTime, double remainingTime)
{
    public double CurrentTime { get; set; } = currentTime; // In seconds.
    public double DurationTime { get; set; } = durationTime; // In seconds.
    public double RemainingTime { get; set; } = remainingTime; // In seconds. // TODO: Make this a method? Since remaining time is always the duration - currentTime.
}