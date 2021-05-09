using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Midos.Services.Logging
{
  public class MidosLogger : ILogger
  {
    private readonly object _lock = new();

    private readonly string _name;

    private bool _isClearing = false;

    private readonly ConcurrentBag<LogData> _logData = new();

    public MidosLogger(string name)
    {
      _name = name;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
      return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      return true;
    }

    public void Log<TState>(
      LogLevel logLevel,
      EventId eventId,
      TState state,
      Exception exception,
      Func<TState, Exception, string> formatter
    ) {
      var log = new LogData {
        Category = _name,
        Level = logLevel.ToString(),
        EventId = eventId.ToString(),
        Exception = ConvertException(exception),
        Data = ConvertLogState(state),
        CreatedAt = DateTime.Now,
      };

      if (_isClearing) {
        lock(_lock) {
          _logData.Add(log);
        }
      } else {
        _logData.Add(log);
      }
    }

    public LogData[] Clear()
    {
      LogData[] result;

      _isClearing = true;

      lock(_lock) {
        result = _logData.ToArray();
        _logData.Clear();
      }

      _isClearing = false;

      return result;
    }

    public LogData[] Get()
      => _logData.ToArray();

    private static object ConvertException(Exception exception, int deep = 0)
      => exception is null ? null : new {
        exception.Data,
        exception.Message,
        exception.Source,
        exception.HResult,
        exception.HelpLink,
        StackTrace = exception.StackTrace?.Split('\n').Select(row => row.Trim()),
        InnerException = deep > 10 ? null : ConvertException(exception.InnerException, deep + 1)
      };

    private static object ConvertLogState(object state)
    {
      var pairs = state as IEnumerable<KeyValuePair<string, object>>;

      return new {
        Keys = pairs.Select(kv => kv.Key).ToArray(),
        Values = pairs.ToDictionary(
          kv => kv.Key,
          kv => ConvertLogStateValue(kv.Value)
        )
      };
    }

    private static object ConvertLogStateValue(object value)
    {
      try {
        JsonSerializer.Serialize(value);

        return value;
      } catch {
        return value.ToString();
      }
    }
  }
}
