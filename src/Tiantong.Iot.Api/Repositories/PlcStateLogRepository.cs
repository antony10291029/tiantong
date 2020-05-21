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
      var stateIds = _db.PlcStates
        .Where(s => s.plc_id == plcId)
        .Select(s => s.id)
        .ToArray();

      return _db.PlcStateLogs
        .Where(log => log.plc_id == plcId && stateIds.Contains(log.state_id))
        .OrderByDescending(log => log.created_at)
          .ThenBy(log => log.id)
        .Paginate(page, pageSize);
    }
  }
}
