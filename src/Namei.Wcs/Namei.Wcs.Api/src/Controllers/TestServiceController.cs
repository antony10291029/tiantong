using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

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

      return Success("目标楼层已设置");
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
        _cap.Publish(LifterTaskImportedEvent.Message, new LifterTaskImportedEvent(param.lifter_id, param.floor));
      } else if (param.message == "exported") {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent(param.lifter_id, param.floor));
      } else if (param.message == "scanned") {
        _cap.Publish(LifterTaskScannedEvent.Message, new LifterTaskScannedEvent(param.lifter_id, param.floor));
      } else if (param.message == "taken") {
        _cap.Publish(LifterTaskTakenEvent.Message, new LifterTaskTakenEvent(param.lifter_id, param.floor));
      }

      return Success($"指令已发送: {param.lifter_id} 号梯，{param.floor} 楼");
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

      return new { message = "指令已发送" };
    }

    [HttpPost]
    [Route("/test/system-settings")]
    public object GetSystemSettings()
    {
      return new {
        enableDoorsCommands = Config.EnableDoorsCommands,
        enableLifterCommands = Config.EnableLifterCommands,
        enableHoisterCommands = Config.EnableHoistersCommands,
        enableWmsCommands = Config.EnableWmsCommands,
        enableRcsCommands = Config.EnableRcsCommands,
      };
    }

    public class SystemSettingsParams
    {
      public bool enableDoorsCommands { get; set; }

      public bool enableLifterCommands { get; set; }

      public bool enableHoisterCommands { get; set; }

      public bool enableWmsCommands { get; set; }

      public bool enableRcsCommands { get; set; }
    }

    [HttpPost]
    [Route("/test/system-settings/set")]
    public object SetSystemSettings([FromBody] SystemSettingsParams param)
    {
      Config.EnableDoorsCommands = param.enableDoorsCommands;
      Config.EnableLifterCommands = param.enableLifterCommands;
      Config.EnableHoistersCommands = param.enableHoisterCommands;
      Config.EnableWmsCommands = param.enableWmsCommands;
      Config.EnableRcsCommands = param.enableRcsCommands;

      return Success("系统设置已保存");
    }
  }
}
