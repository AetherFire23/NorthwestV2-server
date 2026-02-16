using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ERP.Integration;

// TODO: Put this into the Commons library 
public class XUnitLoggerProvider : ILoggerProvider
{
    private readonly ITestOutputHelper _output;

    public XUnitLoggerProvider(ITestOutputHelper output)
    {
        _output = output;
    }

    /// <summary>
    /// From interface ILoggerProvider, needs to accept a categoryName.
    /// CategoryName sseems to be provided by the logging framework. 
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName) => new XUnitLogger(_output, categoryName);

    public void Dispose()
    {
    }
}

public class XUnitLogger : ILogger
{
    private readonly ITestOutputHelper _output;
    private readonly string _category;

    public XUnitLogger(ITestOutputHelper output, string category)
    {
        _output = output;
        _category = category;
    }

    public IDisposable BeginScope<TState>(TState state) => default!;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception?, string> formatter)
    {
        //TODO : find a way to make it more easy to modify. 
        // Ignoring all entity framework and indesirable 
        if (_category.Contains("EntityFrameworkCore"))
        {
            return;
        }

        _output.WriteLine($"{Environment.NewLine}<{_category}> {Environment.NewLine} {formatter(state, exception)}");
    }
}