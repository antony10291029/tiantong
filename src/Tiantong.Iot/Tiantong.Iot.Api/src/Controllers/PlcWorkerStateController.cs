using Microsoft.AspNetCore.Mvc;
using Renet.Web;

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

    public class SetStringByIdParams
    {
      public string plc { get; set; }

      public string state { get; set; }

      public string value { get; set; }
    }

    public class GetStringParams
    {
      public string plc { get; set; }

      public string state { get; set; }
    }

    [HttpPost]
    [Route("get-string")]
    public object GetString([FromBody] GetStringParams param)
    {
      var value = _manager.Get(param.plc).Get(param.state);

      return new { value };
    }

    [HttpPost]
    [Route("set-string")]
    public object SetString([FromBody] SetStringByIdParams param)
    {
      _manager.Get(param.plc).Set(param.state, param.value);

      return SuccessOperation("数据已写入");
    }
  }
}
