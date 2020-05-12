using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class StateErrorLogger
  {
    private IotDbContext _db;

    private readonly object _dbLock = new object();

    public StateErrorLogger(IotDbContext db)
    {
      _db = db;
    }

    public void Log(int plcId, int stateId, string operation, string value, string message)
    {
      var log = new PlcStateError {
        plc_id = plcId,
        state_id = stateId,
        operation = operation,
        value = value,
        message = message
      };

      lock(_dbLock) {
        _db.PlcStateErrors.Add(log);
        _db.SaveChanges();
      }
    }
  }
}
