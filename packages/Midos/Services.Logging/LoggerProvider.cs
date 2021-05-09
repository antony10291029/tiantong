using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Midos.Services.Logging
{
  public class MidosLoggerProvider : ILoggerProvider
  {
    private readonly ConcurrentDictionary<string, MidosLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
      return _loggers.GetOrAdd(categoryName, name => new(name));
    }

    public MidosLogger[] Loggers()
      => _loggers.Values.ToArray();

    public void Dispose()
    {
      _loggers.Clear();
      GC.SuppressFinalize(this);
    }
  }
}
