using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcWorkerLogger: IPlcWorkerLogger
  {
    private IotDbContext _db;

    private readonly object _dbLock = new object();

    public PlcWorkerLogger(IotDbContext db)
    {
      _db = db;
    }

    public void Log(int plcId, string message)
    {
      var log = new PlcLog {
        plc_id = plcId,
        message = message
      };

      lock (_dbLock) {
        _db.Add(log);
        _db.SaveChanges();
      }
    }

  }

}
