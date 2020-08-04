using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class TestServiceController: BaseController
  {
    private LifterServiceManager _lifters;

    public TestServiceController(LifterServiceManager lifters)
    {
      _lifters = lifters;
    }

    [HttpPost]
    [Route("/test/lifters/destination")]
    public object GetDestination()
    {
      return new {
        destination = WmsService.Destination,
      };
    }

    public class SetDestinationParams
    {
      public string destination { get; set; }
    }

    [HttpPost]
    [Route("/test/lifters/set-destination")]
    public object SetDestination([FromBody] SetDestinationParams param)
    {
      WmsService.Destination = param.destination;

      return new {
        message = "目标楼层已设置"
      };
    }

    public class LifterNotifyParams
    {
      public int lifter_id { get; set; }

      public string floor { get; set; }
    }

    [HttpPost]
    [Route("/test/lifters/import")]
    public object LiftersImport([FromBody] LifterNotifyParams param)
    {
      _lifters.Get(param.lifter_id).Release(param.floor);

      return new {
        message = $"放货完成通知完毕: {param.lifter_id} 号梯，{param.floor} 楼"
      };
    }

    [HttpPost]
    [Route("/test/lifters/export")]
    public object LiftersExport([FromBody] LifterNotifyParams param)
    {
      _lifters.Get(param.lifter_id).Pickup(param.floor);

      return new {
        message = $"取货完成通知完毕: {param.lifter_id} 号梯，{param.floor} 楼"
      };
    }

    [HttpPost]
    [Route("/test/enable-lifter-commands")]
    public object GetLifterCommandEnable()
    {
      return new { value = Config.EnableLifterCommands };
    }

    [HttpPost]
    [Route("/test/enable-doors-commands")]
    public object GetDoorsCommandEnable()
    {
      return new { value = Config.EnableDoorsCommands };
    }

    public class EnableParams
    {
      public bool value { get; set; }
    }

    [HttpPost]
    [Route("/test/set-enable-lifter-commands")]
    public object SetLifterCommandEnable([FromBody] EnableParams param)
    {
      Config.EnableLifterCommands = param.value;

      return new { message = "货梯命令设置完毕" };
    }

    [HttpPost]
    [Route("/test/set-enable-doors-commands")]
    public object SetDoorsCommandEnable([FromBody] EnableParams param)
    {
      Config.EnableDoorsCommands = param.value;

      return new { message = "自动门命令设置完毕" };
    }
  }
}
