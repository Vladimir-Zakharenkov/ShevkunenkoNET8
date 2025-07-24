public interface ITimeService
{
    string GetTime();
}

public class TimeService : ITimeService
{
    public string GetTime() => DateTime.Now.ToShortTimeString();
}