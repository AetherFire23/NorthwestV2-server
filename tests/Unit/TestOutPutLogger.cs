using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ERP.Testing.Domain;

public sealed class TestOutputLogger<T> : ILogger<T>
{
    private readonly ITestOutputHelper _output;
    private readonly string _categoryName;

    public TestOutputLogger(ITestOutputHelper output)
    {
        _output = output;
        _categoryName = typeof(T).FullName ?? typeof(T).Name;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);

        try
        {
            _output.WriteLine(
                $"[{logLevel}] {_categoryName}: {message}");

            if (exception is not null)
            {
                _output.WriteLine(exception.ToString());
            }
        }
        catch (InvalidOperationException)
        {
            // Happens when output is used outside test lifetime.
            // Ignore silently.
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}