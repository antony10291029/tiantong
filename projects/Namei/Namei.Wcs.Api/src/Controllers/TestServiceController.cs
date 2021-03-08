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

      return NotifyResult.FromVoid().Success("目标楼层已设置");
    }

    public class LifterNotifyParams
    {
      public string lifter_id { get; set; }

      public string floor { get; set; }

      public string message { get; set; }
    }

    [HttpPost]
    [Route("/test/lifters/publish-message")]
    public object PublishMessage([FromBody] LifterNotifyParams param)
    {
      if (param.message == "imported") {
        _lifters.Get(param.lifter_id).SetImported(param.floor, true);
      } else if (param.message == "exported") {
        _cap.Publish(LifterTaskExported.Message, LifterTaskExported.From(param.lifter_id, param.floor));
      } else if (param.message == "scanned") {
        _cap.Publish(LifterTaskScannedEvent.Message, new LifterTaskScannedEvent(param.lifter_id, param.floor));
      } else if (param.message == "taken") {
        _lifters.Get(param.lifter_id).SetPickuped(param.floor, true);
      }

      return NotifyResult.FromVoid().Success($"指令已发送: {param.lifter_id} 号梯，{param.floor} 楼");
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
        _cap.PublishAsync(DoorTaskRequestOpenEvent.Message, new DoorTaskRequestOpenEvent(param.door_id, "A0001"));
      } else if (param.message  == "requested.close") {
        _cap.PublishAsync(DoorTaskRequestCloseEvent.Message, new DoorTaskRequestCloseEvent(param.door_id, "A0001"));
      } else if (param.message == "opened") {
        _cap.PublishAsync(DoorOpenedEvent.Message, new DoorOpenedEvent(param.door_id));
      } else if (param.message == "closed") {
        _cap.PublishAsync(DoorClosedEvent.Message, new DoorClosedEvent(param.door_id));
      }

      return NotifyResult.FromVoid().Success("指令已发送");
    }
  }
}
