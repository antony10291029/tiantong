using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskTypeController: BaseController
  {
    private readonly WcsContext _context;

    public AgcTaskTypeController(WcsContext context)
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

    [HttpPost("/agc-task-types/create")]
    public INotifyResult<IMessageObject> Create([FromBody] CreateParams param)
    {
      var result = NotifyResult.FromVoid();

      if (_context.Set<AgcTaskType>().Any(type => type.Key == param.Key)) {
        return result.Danger("任务类型已存在");
      }

      var type = AgcTaskType.From(
        key: param.Key,
        name: param.Name,
        method: param.Method,
        webhook: param.Webhook
      );
      _context.Add(type);
      _context.SaveChanges();

      return result.Success("任务类型已创建");
    }

    [HttpPost("/agc-task-types/update")]
    public INotifyResult<IMessageObject> Update([FromBody] CreateParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Find<AgcTaskType>(param.Id);

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

    [HttpPost("/agc-task-types/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Find<AgcTaskType>(param.Id);

      if (type is null) {
        return result.Danger("任务类型不存在");
      }

      _context.Remove(type);
      _context.SaveChanges();

      return result.Success("任务类型已删除");
    }

    [HttpPost("/agc-task-types/all")]
    public IDataMap<AgcTaskType> All([FromBody] QueryParams param)
    {
      var query = _context.Set<AgcTaskType>().AsQueryable();

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
