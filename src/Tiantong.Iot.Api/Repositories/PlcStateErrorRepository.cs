using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcStateErrorRepository
  {
    private IotDbContext _db;

    public PlcStateErrorRepository(IotDbContext db)
    {
      _db = db;
    }

    public Pagination<PlcStateError> PaginateByPlcId(int plcId, int page, int pageSize)
    {
      var stateIds = _db.PlcStates
        .Where(s => s.plc_id == plcId)
        .Select(s => s.id)
        .ToArray();

      return _db.PlcStateErrors
        .Where(error => error.plc_id == plcId && stateIds.Contains(error.state_id))
        .OrderByDescending(error => error.id)
        .Paginate(page, pageSize);
    }
  }
}
