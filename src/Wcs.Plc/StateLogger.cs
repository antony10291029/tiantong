using System.Text.Json;
using System.Collections.Generic;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class StateLogger : IStatePlugin
  {
    private DbContext _db;

    private PlcConnection _plcConnection;

    public Interval Interval = new Interval();

    private List<PlcStateLog> _stateLogs = new List<PlcStateLog>();

    public StateLogger(IntervalManager manager, DbContext dbContext, PlcConnection plcConnection)
    {
      _db = dbContext;
      _plcConnection = plcConnection;

      manager.Add(Interval);
      Interval.SetTime(500);
      Interval.SetHandler(HandleStateLogs);
    }

    public void HandleStateLogs()
    {
      PlcStateLog[] logs;

      lock (_stateLogs) {
        logs = _stateLogs.ToArray();
        _stateLogs.Clear();
      }

      _db.PlcStateLogs.AddRange(logs);
      _db.SaveChanges();
    }

    public void Install<T>(State<T> state)
    {
      state.AddGetHook(value => {
        var log = new PlcStateLog {
          Operation = "read",
          PlcId = _plcConnection.Id,
          Name = state.Name,
          Key = state.Address,
          Length = state.Length,
          Value = JsonSerializer.Serialize(value),
        };

        lock (_stateLogs) {
          _stateLogs.Add(log);
        }
      });

      state.AddSetHook(value => {
        var log = new PlcStateLog {
          Operation = "write",
          PlcId = _plcConnection.Id,
          Name = state.Name,
          Key = state.Address,
          Length = state.Length,
          Value = JsonSerializer.Serialize(value),
        };

        lock (_stateLogs) {
          _stateLogs.Add(log);
        }
      });
    }
  }
}
