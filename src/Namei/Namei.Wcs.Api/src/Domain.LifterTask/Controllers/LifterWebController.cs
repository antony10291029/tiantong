using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterWebController: BaseController
  {
    private ICapPublisher _cap;

    private ILifterServiceFactory _lifters;

    private DeviceErrorService _deviceErrorService;

    public LifterWebController(
      ICapPublisher cap,
      ILifterServiceFactory lifters,
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
