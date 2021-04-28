using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskTypeController: BaseController
  {
    private readonly WcsContext _context;

    public RcsAgcTaskTypeController(WcsContext context)
    {
      _context = context;
    }

    public struct CreateParams
    {
      public long Id { get; set; }

      public string Key { get; set; }

      public string Name { get; set; }

      public string Method { get; set; }

      public string Webhook { get; set; }
    }

    [HttpPost("/rcs-agc-task-type/create")]
    public INotifyResult<IMessageObject> Create([FromBody] CreateParams param)
    {
      var result = NotifyResult.FromVoid();

      if (_context.Set<RcsAgcTaskType>().Any(type => type.Key == param.Key)) {
        return result.Danger("任务类型已存在");
      }

      var type = RcsAgcTaskType.From(
        key: param.Key,
        name: param.Name,
        method: param.Method,
        webhook: param.Webhook
      );
      _context.Add(type);
      _context.SaveChanges();

      return result.Success("任务类型已创建");
    }

    [HttpPost("/rcs-agc-task-type/update")]
    public INotifyResult<IMessageObject> Update([FromBody] CreateParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Find<RcsAgcTaskType>(param.Id);

      type.Update(
        key: param.Key,
        name: param.Name,
        method: param.Method,
        webhook: param.Webhook
      );
      _context.SaveChanges();

      return result.Success("任务类型已更新");
    }

    public struct DeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/rcs-agc-task-type/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Find<RcsAgcTaskType>(param.Id);

      if (type is null) {
        return result.Danger("任务类型不存在");
      }

      _context.Remove(type);
      _context.SaveChanges();

      return result.Success("任务类型已删除");
    }

    [HttpPost("/rcs-agc-task-type/all")]
    public IDataMap<RcsAgcTaskType> All([FromBody] QueryParams param)
    {
      var query = _context.Set<RcsAgcTaskType>().AsQueryable();

      if (param.Query != "" && param.Query != null) {
        query = query.Where(type =>
          type.Key.Contains(param.Query) ||
          type.Name.Contains(param.Query) ||
          type.Webhook.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(query => query.Id)
        .ToDataMap();
    }
  }
}
