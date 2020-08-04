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
      public int destination { get; set ;}
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
      _lifters.Get(param.lifter_id).SetDestination(param.floor, WmsService.Destination.ToString());

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
  }
}
