using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcWorkerLogger: Logger, IPlcWorkerLogger
  {
    private readonly object _dbLock = new object();

    public void Log(int plcId, string message)
    {
      var log = new PlcLog {
        plc_id = plcId,
        message = message
      };

      lock (_dbLock) {
        DbContext.Add(log);
        DbContext.SaveChanges();
      }
    }

    public void Dispose()
    {
      DbContext.Dispose();
    }

  }

}
