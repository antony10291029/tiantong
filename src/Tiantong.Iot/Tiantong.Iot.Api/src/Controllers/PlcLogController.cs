using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;
using Z.EntityFramework.Plus;

namespace Tiantong.Iot.Api
{
  [Route("/plc-logs")]
  public class PlcLogController: BaseController
  {
    private LogContext _log;

    private SystemContext _system;

    private PlcLogRepository _logRepository;

    public  PlcLogController(
      LogContext log,
      SystemContext system,
      PlcLogRepository logRepository
    ) {
      _log = log;
      _system = system;
      _logRepository = logRepository;
    }

    public class LatestParams
    {
      public int plc_id { get; set; }

      public int page { get; set; } = 1;

      public int pageSize { get; set; } = 10;
    }

    [HttpPost]
    [Route("paginate")]
    public object Paginate([FromBody] LatestParams param)
    {
      return _logRepository.Paginate(param.plc_id, param.page, param.pageSize);
    }

    public class ClearParams
    {
      public int days { get; set; }
    }

    [HttpPost]
    [Route("clear")]
    public object Paginate([FromBody] ClearParams param)
    {
      var date = DateTime.Now.AddDays(0 - param.days);

      _log.PlcLogs.Where(log => log.created_at < date).Delete();
      _log.PlcStateLogs.Where(log => log.created_at < date).Delete();
      _log.PlcStateErrors.Where(log => log.created_at < date).Delete();
      _log.HttpPusherLogs.Where(log => log.created_at < date).Delete();
      _log.HttpPusherErrors.Where(log => log.created_at < date).Delete();

      return SuccessOperation("日志清理完毕");
    }

  }
}
