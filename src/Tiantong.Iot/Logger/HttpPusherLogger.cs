using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class HttpPusherLogger: Logger
  {
    private List<HttpPusherLog> _logs = new List<HttpPusherLog>();

    private List<HttpPusherError> _errorLogs = new List<HttpPusherError>();

    private readonly object _logLock = new object();

    private readonly object _errorLogLock = new object();

    public HttpPusherLogger(IntervalManager manager)
    {
      UseIntervalManager(manager);
    }

    public override void HandleLog()
    {
      HttpPusherLog[] logs;
      HttpPusherError[] errorLogs;

      lock (_logLock) {
        logs = _logs.ToArray();
        _logs.Clear();
      }

      lock (_errorLogs) {
        errorLogs = _errorLogs.ToArray();
        _errorLogs.Clear();
      }

      DbContext.AddRange(logs);
      DbContext.AddRange(errorLogs);
      DbContext.SaveChanges();
    }

    public void Log(HttpPusherLog log)
    {
      lock (_logLock) {
        _logs.Add(log);
      }
    }

    public void LogError(HttpPusherError error)
    {
      lock (_errorLogLock) {
        _errorLogs.Add(error);
      }
    }

  }

}
