using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Midos.Eventing;

namespace Namei.Wcs.Api
{
  public class DoorPlcCommandController: BaseController
  {
    private IEventPublisher _publisher;

    public DoorPlcCommandController(IEventPublisher publisher)
    {
      _publisher = publisher;
    }

    public class DoorStateChangedParams
    {
      [JsonPropertyName("door_id")]
      public string DoorId { get; set; }

      [JsonPropertyName("value")]
      public string Value { get; set; }
    }

    [HttpPost("/doors/state/changed")]
    public object DoorStateChanged([FromBody] DoorStateChangedParams param)
    {
      var message = "指令未识别";

      if (param.Value == "12") {
        message = "正在处理开门完成指令";
        _publisher.Publish(WcsDoorEvent.Opened, WcsDoorEvent.From(param.DoorId));
      } else if (param.Value == "22") {
        message = "正在处理关门完成指令";
        _publisher.Publish(WcsDoorEvent.Closed, WcsDoorEvent.From(param.DoorId));
      }

      return NotifyResult.FromVoid().Success(message);
    }
  }
}
