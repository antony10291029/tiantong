using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcStateErrorRepository
  {
    private LogContext _log;

    private SystemContext _system;

    public PlcStateErrorRepository(LogContext log, SystemContext system)
    {
      _log = log;
      _system = system;
    }

    public Pagination<PlcStateError> PaginateByPlcId(int plcId, int page, int pageSize)
    {
      var stateIds = _system.PlcStates
        .Where(s => s.plc_id == plcId)
        .Select(s => s.id)
        .ToArray();

      return _log.PlcStateErrors
        .Where(error => error.plc_id == plcId && stateIds.Contains(error.state_id))
        .OrderByDescending(error => error.id)
        .Paginate(page, pageSize);
    }

  }

}
