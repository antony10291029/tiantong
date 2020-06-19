using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcLogRepository
  {
    private LogContext _log;

    public PlcLogRepository(LogContext log)
    {
      _log = log;
    }

    public Pagination<PlcLog> Paginate(int plcId, int page, int pageSize)
    {
      return _log.PlcLogs
        .Where(pl => pl.plc_id == plcId)
        .OrderByDescending(pl => pl.id)
        .Paginate(page, pageSize);
    }

  }

}
