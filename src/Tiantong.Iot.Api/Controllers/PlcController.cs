using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/plc")]
  public class PlcController: BaseController
  {
    private PlcRepository _plcRepository;

    public PlcController(PlcRepository plcRepository)
    {
      _plcRepository = plcRepository;
    }

    public class CreateParams: Plc
    {

    }

    [HttpPost]
    [Route("create")]
    public object Create([FromBody] Plc plc)
    {
      _plcRepository.Add(plc);

      return SuccessOperation("PLC已创建");
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("delete")]
    public object Delete([FromBody] DeleteParams param)
    {
      _plcRepository.Delete(param.id);

      return SuccessOperation("PLC已删除");
    }

    [HttpPost]
    [Route("update")]
    public object Update([FromBody] Plc plc)
    {
      _plcRepository.Update(plc);

      return SuccessOperation("数据已删除");
    }

    [HttpPost]
    [Route("all")]
    public object All()
    {
      return _plcRepository.All();
    }
  }
}
