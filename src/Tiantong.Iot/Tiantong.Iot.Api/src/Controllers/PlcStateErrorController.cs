using Microsoft.AspNetCore.Mvc;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("plcs")]
  public class PlcStateErrorController: BaseController
  {
    private PlcStateErrorRepository _errorRepository;

    public PlcStateErrorController(PlcStateErrorRepository errorRepository)
    {
      _errorRepository = errorRepository;
    }

    public class PaginateByPlc
    {
      public int plc_id { get; set; }

      public int page { get; set; }

      public int page_size { get; set; }
    }

    [HttpPost("state-errors/paginate")]
    public Pagination<PlcStateError> PaginateByPlcId([FromBody] PaginateByPlc param)
    {
      return _errorRepository.PaginateByPlcId(param.plc_id, param.page, param.page_size);
    }

  }
}
