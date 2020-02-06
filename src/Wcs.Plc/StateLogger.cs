using System;
using System.Text.Json;
using System.Collections.Generic;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class StateLogger : IStatePlugin
  {
    private DbContext _db;

    private IPlcContainer _container;

    private List<PlcStateLog> _stateLogs = new List<PlcStateLog>();

    public IInterval Interval;

    public StateLogger(IPlcContainer container)
    {
      _container = container;
      _db = container.ResolveDbContext();

      Interval = new Interval();
      Interval.SetTime(500);
      Interval.SetHandler(HandleStateLogs);
      container.IntervalManager.Add(Interval);
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

    public void Install<T>(IState<T> state)
    {
      state.AddGetHook(value => {
        var log = new PlcStateLog {
          Operation = "read",
          PlcId = _container.PlcConnection.Id,
          Name = state.Name,
          Key = state.Key,
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
          PlcId = _container.PlcConnection.Id,
          Name = state.Name,
          Key = state.Key,
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
