using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class WmsWebController: BaseController
  {
    private ICapPublisher _cap;


    public WmsWebController(ICapPublisher cap)
    {
      _cap = cap;
    }

    public class NotifyParams
    {
      public string method { get; set; }

      public string floor { get; set; }

      public string liftCode { get; set; }

      public string taskid { get; set; }
    }

    [HttpPost("/lifters/imported")]
    public object Imported([FromBody] NotifyParams param)
    {
      if (param.method == "deliver") {
        _cap.Publish(LifterTaskImportedEvent.Message, new LifterTaskImportedEvent(param.liftCode, param.floor, param.taskid));
      } else if (param.method == "pickup") {
        _cap.Publish(LifterTaskTakenEvent.Message, new LifterTaskTakenEvent(param.liftCode, param.floor, param.taskid));
      } else {
        return "method 未识别";
      }

      var methodString = param.method == "deliver" ? "放货" : "取货";

      return new { message = $"{methodString}信号已收到，LifterId: {param.liftCode}, Floor: {param.floor}" };
    }
  }
}
