using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Api;
using System.Text.Json;

namespace Namei.Wcs.Aggregates
{
  public class WmsPickTicketTaskController: BaseController
  {
    public const string OrderType = "wms.export.carry";

    public const string Group = "WmsPickTicketTaskController";

    private WcsContext _context;

    private IAppConfig _config;

    public WmsPickTicketTaskController(IAppConfig config, WcsContext context)
    {
      _config = config;
      _context = context;
    }

    public struct StartParams
    {
      public long TaskId { get; set; }

      public string Position { get; set; }

      public string Destination { get; set; }

      public string PalletCode { get; set; }
    }

    [HttpPost("/wms-pick-ticket-tasks/start")]
    public void Start([FromBody] StartParams param)
    {
      _context.Publish(
        name: RcsAgcTaskCreate.Message,
        data: RcsAgcTaskCreate.From(
          taskType: RcsAgcTaskType.Carry,
          position: param.Position,
          destination: param.Destination,
          podCode: param.PalletCode,
          orderType: OrderType,
          orderId: param.TaskId
        )
      );
    }

    [HttpPost("/wms-pick-ticket-tasks/finish")]
    public object HandleFinish(RcsAgcTaskOrderFinished param)
    {
      Finished(param);

      return new { message = "任务已完成" };
    }

    [RcsAgcTaskOrderFinished(OrderType, Group = Group)]
    public void Finished(RcsAgcTaskOrderFinished param)
    {
      var url = "http://localhost:5300/";

      if (_config.IsProduction) {
        url = "http://172.16.2.64:5300/";
      } else if (!_config.IsDevelopment) {
        url = "http://172.16.2.74:5300/";
      }

      _context.Publish(
        name: WebHookPost.Message,
        data: new WebHookPost(
          url: $"{url}/pick-ticket-task/finish",
          data: JsonSerializer.Serialize(new { TaskId = param.OrderId })
        )
      );
    }
  }
}
