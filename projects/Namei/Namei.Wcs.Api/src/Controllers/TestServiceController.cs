using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class TestServiceController: BaseController
  {
    private ICapPublisher _cap;
    
    private LifterServiceManager _lifters;

    public TestServiceController(LifterServiceManager lifters, ICapPublisher cap)
    {
      _cap = cap;
      _lifters = lifters;
    }

    public class PublishDoorsMessageParams
    {
      public string door_id { get; set; }

      public string message { get; set; }
    }

    [HttpPost]
    [Route("/test/doors/publish-message")]
    public object PublishDoorsMessage([FromBody] PublishDoorsMessageParams param)
    {
      if (param.message == "requested.open") {
        _cap.PublishAsync(
          RcsDoorEvent.Request,
          RcsDoorEvent.From(
            uuid: "A0001",
            doorId: param.door_id
          )
        );
      } else if (param.message  == "requested.close") {
        _cap.PublishAsync(
          RcsDoorEvent.Leave,
          RcsDoorEvent.From(
            uuid: "A0001",
            doorId: param.door_id
          )
        );
      } else if (param.message == "opened") {
        _cap.PublishAsync(
          WcsDoorEvent.Opened,
          WcsDoorEvent.From(param.door_id)
        );
      } else if (param.message == "closed") {
        _cap.PublishAsync(
          WcsDoorEvent.Closed,
          WcsDoorEvent.From(param.door_id)
        );
      }

      return NotifyResult.FromVoid().Success("指令已发送");
    }
  }
}
