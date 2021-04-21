using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Api;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public class LifterAgcTaskController: BaseController
  {
    private WcsContext _context;

    public LifterAgcTaskController(WcsContext context)
    {
      _context = context;
    }

    public class CreateParams
    {
      public string Type { get; set; }

      public string OrderId { get; set; }

      public string PalletCode { get; set; }

      public string Position { get; set; }

      public string Destination { get; set; }

      public string LifterId { get; set; }
    }

    [HttpPost("/lifter-agc-tasks/create")]
    public INotifyResult<IMessageObject> Create([FromBody] CreateParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Set<LifterAgcTaskType>()
        .FirstOrDefault(type => type.Key == param.Type);

      if (type == null) {
        return result.Danger("任务类型不存在");
      }

      var task = LifterAgcTask.From(
        orderId: param.OrderId,
        palletCode: param.PalletCode,
        position: param.Position,
        destination: param.Destination,
        lifterId: param.LifterId,
        type: type
      );

      _context.Add(task);
      _context.SaveChanges();
      _context.Publish(
        LifterAgcTaskEvent.Created,
        LifterAgcTaskEvent.From(task.Id)
      );

      return result.Success("任务已创建");
    }

    public struct CloseParams
    {
      public long Id { get; init; }
    }

    public INotifyResult<IMessageObject> Close(CloseParams param)
    {
      var result = NotifyResult.FromVoid();
      var task = _context.Find<LifterAgcTask>(param.Id);

      if (task == null) {
        return result.Danger("任务不存在");
      }

      task.Close();
      _context.SaveChanges();
      _context.Publish(
        LifterAgcTaskEvent.Closed,
        LifterAgcTaskEvent.From(task.Id)
      );

      return result.Success("任务已关闭");
    }
  }
}
