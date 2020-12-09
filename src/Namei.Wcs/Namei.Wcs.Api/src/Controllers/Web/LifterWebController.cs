using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterWebController: BaseController
  {
    private ICapPublisher _cap;

    private LifterServiceManager _lifters;

    private DeviceErrorService _deviceErrorService;

    public LifterWebController(
      ICapPublisher cap,
      LifterServiceManager lifters,
      DeviceErrorService deviceErrorService
    ) {
      _cap = cap;
      _lifters = lifters;
      _deviceErrorService = deviceErrorService;
    }

    [HttpPost]
    [Route("/lifters/states")]
    public object GetLifterStates()
    {
      return _lifters.All().ToDictionary(kv => kv.Key, kv => kv.Value.GetStates());
    }

    public class Params
    {
      public string LifterId { get; set; }

      public string Floor { get; set; }
    }

    [HttpPost("/lifters/imported")]
    public object Imported([FromBody] Params param)
    {
      _cap.Publish(LifterTaskImportedEvent.Message, new LifterTaskImportedEvent(param.LifterId,  param.Floor));

      return new { message = "放货完成信号已发送" };
    }

    [HttpPost("/lifters/taken")]
    public object Taken([FromBody] Params param)
    {
      _cap.Publish(LifterTaskTakenEvent.Message, new LifterTaskTakenEvent(param.LifterId,  param.Floor));

      return new { message = "取货完成信号已发送" };
    }

    public class ErrorParams
    {
      public string deviceKey { get; set; }

      public int error { get; set; }
    }

    [HttpPost("/lifters/error")]
    public object Error([FromBody] ErrorParams param)
    {
      if (param.error == 1) {
        param.error = 0;
      }

      _deviceErrorService.Log(param.deviceKey, param.error);

      return new { message = "异常已记录" };
    }
  }
}
