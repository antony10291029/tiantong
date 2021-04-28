using Microsoft.AspNetCore.Mvc;
using Midos.Services.Http;
using Namei.Wcs.Api;
using System.Text.Json;

namespace Namei.Wcs.Aggregates
{
  public class WmsPickTicketTaskController: BaseController
  {
    public const string OrderType = "wms.export.carry";

    public const string Group = "WmsPickTicketTaskController";

    private WcsContext _context;

    private string _url;

    public WmsPickTicketTaskController(IAppConfig config, WcsContext context)
    {
      var url = "http://localhost:5300";

      if (config.IsProduction) {
        url = "http://172.16.2.64:5300";
      } else if (!config.IsDevelopment) {
        url = "http://172.16.2.74:5300";
      }

      _url = url;
      _context = context;
    }

    public struct StartParams
    {
      public long TaskId { get; set; }

      public string Position { get; set; }

      public string Destination { get; set; }

      public string PalletCode { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/start")]
    public object Start([FromBody] StartParams param)
    {
      _context.Publish(
        name: RcsAgcTaskCreate.Message,
        data: RcsAgcTaskCreate.From(
          taskType: RcsAgcTaskMethod.Carry,
          position: param.Position,
          destination: param.Destination,
          podCode: param.PalletCode,
          orderType: OrderType,
          orderId: param.TaskId
        )
      );

      return NotifyResult
        .FromVoid()
        .Success("任务已下发");
    }

    [HttpPost("/wms/pick-ticket-tasks/finish")]
    public object HandleFinish([FromBody] RcsAgcTaskOrderFinished param)
    {
      Finished(param);

      return NotifyResult
        .FromVoid()
        .Success("任务已完成");
    }

    [RcsAgcTaskOrderFinished(OrderType, Group = Group)]
    public void Finished(RcsAgcTaskOrderFinished param)
    {
      _context.Publish(
        name: HttpPost.Event,
        data: HttpPost.From(
          url: $"{_url}/wms/pick-ticket-tasks/finish",
          data: new { Id = param.OrderId }
        )
      );
    }
  }
}
