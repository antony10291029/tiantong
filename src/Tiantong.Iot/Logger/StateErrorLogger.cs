using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class StateErrorLogger: Logger
  {
    private readonly object _dbLock = new object();

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
        DbContext.PlcStateErrors.Add(log);
        DbContext.SaveChanges();
      }
    }

  }

}
