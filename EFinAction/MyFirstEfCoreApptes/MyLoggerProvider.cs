using Microsoft.Extensions.Logging;

namespace MyFirstEfCoreApp;

public class MyLoggerProvider(List<string> logs) : ILoggerProvider
{
    private readonly List<string> _logs = logs;

    public ILogger CreateLogger(string categoryName)
    {
        return new MyLogger(_logs);
    }

    void IDisposable.Dispose()
    {
    }

    private class MyLogger(List<string> logs) : ILogger
    {
        private readonly List<string> _logs = logs;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            _logs.Add(formatter(state, exception));
            //Console.WriteLine(formatter(state, exception));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}