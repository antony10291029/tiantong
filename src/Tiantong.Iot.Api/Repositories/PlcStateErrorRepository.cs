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
      return _db.PlcStateErrors
        .Where(error => error.plc_id == plcId)
        .Paginate(page, pageSize);
    }
  }
}
