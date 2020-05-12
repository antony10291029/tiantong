using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcStateLogRepository
  {
    private IotDbContext _db;

    public PlcStateLogRepository(IotDbContext db)
    {
      _db = db;
    }

    public Pagination<PlcStateLog> PaginateByPlcId(int plcId, int page, int pageSize)
    {
      return _db.PlcStateLogs
        .Where(log => log.plc_id == plcId)
        .OrderByDescending(log => log.id)
        .Paginate(page, pageSize);
    }
  }
}
