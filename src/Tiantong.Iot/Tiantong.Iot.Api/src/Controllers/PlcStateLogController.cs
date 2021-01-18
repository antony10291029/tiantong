using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("plcs")]
  public class PlcStateLogController: BaseController
  {
    private PlcStateLogRepository _logRepository;

    public PlcStateLogController(PlcStateLogRepository logRepository)
    {
      _logRepository = logRepository;
    }

    public class PaginateByPlc
    {
      public int plc_id { get; set; }

      public int page { get; set; }

      public int page_size { get; set; }
    }

    [HttpPost("state-logs/paginate")]
    public Pagination<PlcStateLog> PaginateByPlcId([FromBody] PaginateByPlc param)
    {
      return _logRepository.PaginateByPlcId(param.plc_id, param.page, param.page_size);
    }

  }
}
