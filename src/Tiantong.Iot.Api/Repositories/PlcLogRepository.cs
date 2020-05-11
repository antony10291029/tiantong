using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcLogRepository
  {
    private IotDbContext _db;

    public PlcLogRepository(IotDbContext db)
    {
      _db = db;
    }

    public Pagination<PlcLog> Paginate(int plcId, int page, int pageSize)
    {
      return _db.PlcLogs
        .Where(pl => pl.plc_id == plcId)
        .OrderByDescending(pl => pl.id)
        .Paginate(page, pageSize);
    }
  }
}
