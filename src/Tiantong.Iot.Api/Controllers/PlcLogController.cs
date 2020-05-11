using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  [Route("/plcs/logs")]
  public class PlcLogController: BaseController
  {
    private PlcLogRepository _logRepository;

    public  PlcLogController(PlcLogRepository logRepository)
    {
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
  }
}
