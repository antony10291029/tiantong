using System.Text.Json;
using System.Collections.Generic;
using Tiantong.Iot.Database;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class StateLogger : IStatePlugin
  {
    private DbContext _db;

    private PlcWorker _plc;

    public Interval Interval = new Interval();

    private List<PlcStateLog> _stateLogs = new List<PlcStateLog>();

    public StateLogger(PlcWorker plc, IntervalManager manager, DbContext dbContext)
    {
      _plc = plc;
      _db = dbContext;

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
        // var log = new PlcStateLog {
        //   plc_id = _plcConnection.Id,
        //   state_id = _plcConnection.Id,
        //   operation = PlcStateOperation.Read,
        //   Name = state.Name,
        //   Key = state.Address,
        //   Length = state.Length,
        //   Value = JsonSerializer.Serialize(value),
        // };

        lock (_stateLogs) {
          // _stateLogs.Add(log);
        }
      });

      state.AddSetHook(value => {
        // var log = new PlcStateLog {
        //   Operation = "write",
        //   PlcId = _plcConnection.Id,
        //   Name = state.Name,
        //   Key = state.Address,
        //   Length = state.Length,
        //   Value = JsonSerializer.Serialize(value),
        // };

        lock (_stateLogs) {
          // _stateLogs.Add(log);
        }
      });
    }
  }
}
