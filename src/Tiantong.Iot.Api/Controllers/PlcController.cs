using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/plcs")]
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

    public class FindParams
    {
      public int plc_id { get; set; }
    }

    [HttpPost]
    [Route("delete")]
    public object Delete([FromBody] FindParams param)
    {
      _plcRepository.Delete(param.plc_id);

      return SuccessOperation("PLC已删除");
    }

    [HttpPost]
    [Route("update")]
    public object Update([FromBody] Plc plc)
    {
      _plcRepository.Update(plc);

      return SuccessOperation("PLC配置已更新");
    }

    [HttpPost]
    [Route("find")]
    public object Find([FromBody] FindParams param)
    {
      return _plcRepository.EnsureGet(param.plc_id);
    }

    [HttpPost]
    [Route("all")]
    public object All()
    {
      return _plcRepository.All();
    }
  }
}
