using System.Text.Json;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class StateLogger : Logger, IStatePlugin
  {
    private IotDbContext _db;

    private int _plcId;

    public Interval Interval = new Interval();

    private List<PlcStateLog> _stateLogs = new List<PlcStateLog>();

    public StateLogger(int plcId, IntervalManager manager, IotDbContext dbContext): base(manager)
    {
      _plcId = plcId;
      _db = dbContext;
    }

    public override void HandleLog()
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
          plc_id = _plcId,
          state_id = state._id,
          operation = "get",
          value = JsonSerializer.Serialize(value),
        };

        lock (_stateLogs) {
          _stateLogs.Add(log);
        }
      });

      state.AddSetHook(value => {
        var log = new PlcStateLog {
          plc_id = _plcId,
          state_id = state._id,
          operation = "set",
          value = JsonSerializer.Serialize(value),
        };

        lock (_stateLogs) {
          _stateLogs.Add(log);
        }
      });
    }
  }
}
