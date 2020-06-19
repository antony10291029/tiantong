using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcStateLogRepository
  {
    private LogContext _log;

    private SystemContext _system;

    public PlcStateLogRepository(LogContext log, SystemContext system)
    {
      _log = log;
      _system = system;
    }

    public Pagination<PlcStateLog> PaginateByPlcId(int plcId, int page, int pageSize)
    {
      var stateIds = _system.PlcStates
        .Where(s => s.plc_id == plcId)
        .Select(s => s.id)
        .ToArray();

      return _log.PlcStateLogs
        .Where(log => log.plc_id == plcId && stateIds.Contains(log.state_id))
        .OrderByDescending(log => log.created_at)
          .ThenBy(log => log.id)
        .Paginate(page, pageSize);
    }
  }
}
