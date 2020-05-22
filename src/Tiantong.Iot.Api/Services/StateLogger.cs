using System.Text.Json;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class StateLogger
  {
    private IotDbContext _db;

    public StateLogger(IotDbContext db)
    {
      _db = db;
    }

    public void LogWrite<T>(int plcId, int stateId, T value)
    {
      var log = new PlcStateLog {
        plc_id = plcId,
        state_id = stateId,
        operation = StateOperation.Write,
        value = JsonSerializer.Serialize(value)
      };

      _db.Add(log);
      _db.SaveChanges();
    }

    public void LogRead<T>(int plcId, int stateId, T value)
    {
      var log = new PlcStateLog {
        plc_id = plcId,
        state_id = stateId,
        operation = StateOperation.Read,
        value = JsonSerializer.Serialize(value),
      };

      _db.Add(log);
      _db.SaveChanges();
    }

  }

}
