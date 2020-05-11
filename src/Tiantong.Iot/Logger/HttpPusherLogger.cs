using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class HttpPusherLogger: Logger
  {
    private IotDbContext _db;

    private List<HttpPusherLog> _logs = new List<HttpPusherLog>();

    private List<HttpPusherError> _errorLogs = new List<HttpPusherError>();

    private readonly object _logLock = new object();

    private readonly object _errorLogLock = new object();

    public HttpPusherLogger(IotDbContext db, IntervalManager manager, int interval = 500): base(manager, interval)
    {
      _db = db;
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

      _db.AddRange(logs);
      _db.AddRange(errorLogs);
      _db.SaveChanges();
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
