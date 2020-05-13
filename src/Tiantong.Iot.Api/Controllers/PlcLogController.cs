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
    private IotDbContext _db;

    private PlcLogRepository _logRepository;

    public  PlcLogController(PlcLogRepository logRepository, IotDbContext db)
    {
      _db = db;
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

      _db.PlcLogs.Where(log => log.created_at < date).Delete();
      _db.PlcStateLogs.Where(log => log.created_at < date).Delete();
      _db.PlcStateErrors.Where(log => log.created_at < date).Delete();
      _db.HttpPusherLogs.Where(log => log.created_at < date).Delete();
      _db.HttpPusherErrors.Where(log => log.created_at < date).Delete();

      return SuccessOperation("日志清理完毕");
    }

  }
}
