using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Collections.Generic;

namespace Tiantong.Iot.Api
{
  [Route("/plc-states")]
  public class PlcWorkerStateController: BaseController
  {
    private PlcManager _manager;

    public PlcWorkerStateController(PlcManager manager)
    {
      _manager = manager;
    }

    public class GetParams
    {
      public string plc { get; set; }

      public string state { get; set; }
    }

    public class SetParams: GetParams
    {
      public string value { get; set; }
    }

    [HttpPost("get")]
    public object GetString([FromBody] GetParams param)
    {
      var value = _manager.Get(param.plc).Get(param.state);

      return new { value };
    }

    [HttpPost("set")]
    public object SetString([FromBody] SetParams param)
    {
      _manager.Get(param.plc).Set(param.state, param.value);

      return SuccessOperation("数据已写入");
    }

    [HttpPost("collect")]
    public object CollectString([FromBody] GetParams param)
    {
      var value = _manager.Get(param.plc).Collect(param.state, 1000);

      return new { value };
    }

    public class GetValuesParams
    {
      public string plc { get; set; }
    }

    [HttpPost("values")]
    public object GetValues([FromBody] GetValuesParams param)
    {
      try {
        return new {
          data = _manager.Get(param.plc).GetValues()
        };
      } catch {
        throw new HttpException("数据不存在", 400);
      }
    }

    [HttpPost("all-values")]
    public Dictionary<string, string> GetAllValues([FromBody] GetValuesParams param)
    {
      try {
        return _manager.Get(param.plc).GetAllValues();
      } catch {
        throw new HttpException("数据不存在", 400);
      }
    }
  }
}
