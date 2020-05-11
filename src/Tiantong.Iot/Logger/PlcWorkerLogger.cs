using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcWorkerLogger: IPlcWorkerLogger
  {
    private IotDbContext _db;

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

      _db.Add(log);
      _db.SaveChanges();
    }

  }

}
