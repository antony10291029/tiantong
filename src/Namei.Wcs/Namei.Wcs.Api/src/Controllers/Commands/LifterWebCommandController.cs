using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class LifterWebCommandController: BaseController
  {
    private ICapPublisher _cap;

    public LifterWebCommandController(ICapPublisher cap)
    {
      _cap = cap;
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
  }
}
