using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
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
      _cap.Publish(LifterTaskImported.Message, LifterTaskImported.From(
        lifterId: param.LifterId,
        floor: param.Floor
      ));

      return NotifyResult.FromVoid().Success("手动发送放货完成指令");
    }

    [HttpPost("/lifters/taken")]
    public object Taken([FromBody] Params param)
    {
      _lifters.Get(param.LifterId).SetPickuped(param.Floor, true);

      return NotifyResult.FromVoid().Success("手动发送取货完成指令");
    }

    public class ErrorParams
    {
      public string device_key { get; set; }

      public string error { get; set; }
    }

    [HttpPost("/standard-lifters/error")]
    public object Error([FromBody] ErrorParams param)
    {
      if (param.error == "1") {
        param.error = "0";
      } else if (param.error == "0") {
        param.error = "1";
      }

      _deviceErrorService.Log(param.device_key, param.error);

      return NotifyResult.FromVoid().Success("异常已记录");
    }
  }
}
