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

    public class GetParams
    {
      public string plc { get; set; }

      public string state { get; set; }
    }

    public class SetParams: GetParams
    {
      public string value { get; set; }
    }

    [HttpPost]
    [Route("get")]
    public object GetString([FromBody] GetParams param)
    {
      var value = _manager.Get(param.plc).Get(param.state);

      return new { value };
    }

    [HttpPost]
    [Route("set")]
    public object SetString([FromBody] SetParams param)
    {
      _manager.Get(param.plc).Set(param.state, param.value);

      return SuccessOperation("数据已写入");
    }
  }
}
