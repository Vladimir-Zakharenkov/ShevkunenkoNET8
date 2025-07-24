namespace DI;

public class TimeFormatter(ITimeService timeService)
{
    ITimeService timeService = timeService;

    public string FormatTime() => $"Current Time: {timeService.GetTime()}";
}