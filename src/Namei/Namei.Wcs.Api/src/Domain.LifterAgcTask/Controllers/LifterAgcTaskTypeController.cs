using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Namei.Wcs.Api;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public class LifterAgcTaskTypeController: BaseController
  {
    private WcsContext _context;

    public LifterAgcTaskTypeController(WcsContext context)
    {
      _context = context;
    }

    public struct CreateParams
    {
      public string Key { get; set; }

      public string Name { get; set; }

      public string WebHook { get; set; }
    }

    [HttpPost("/lifter-agc-task-types/create")]
    public INotifyResult<IMessageObject> Create([FromBody] CreateParams param)
    {
      var type = _context.Set<LifterAgcTaskType>()
        .FirstOrDefault(type => type.Key == param.Key);
      var result = NotifyResult.FromVoid();

      if (type != null) {
        return result.Danger("任务类型已存在");
      }

      _context.Add(new LifterAgcTaskType(
        key: param.Key,
        name: param.Name,
        webHook: param.WebHook
      ));
      _context.SaveChanges();

      return result.Success("任务类型已创建");
    }

    public struct UpdateParams
    {
      public long Id { get; set; }

      public string Key { get; set; }

      public string Name { get; set; }

      public string WebHook { get; set; }
    }

    [HttpPost("/lifter-agc-task-types/update")]
    public INotifyResult<IMessageObject> Update([FromBody] UpdateParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Set<LifterAgcTaskType>().Find(param.Id);

      if (type == null) {
        return result.Danger("任务类型不存在");
      }

      type.Update(param);
      _context.SaveChanges();
      
      return result.Success("任务类型修改成功");
    }

    public class DeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/lifter-agc-task-types/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _context.Set<LifterAgcTaskType>().Find(param.Id);

      if (type == null) {
        return result.Danger("任务类型不存在");
      }

      _context.Remove(type);
      _context.SaveChanges();

      return result.Success("任务类型已删除");
    }

    [HttpPost("/lifter-agc-task-types/paginate")]
    public IPagination<LifterAgcTaskType> Paginate(QueryParams param)
    {
      var query = _context.Set<LifterAgcTaskType>().AsQueryable();

      if (param.Query != null || param.Query != "") {
        query = query.Where(type =>
          type.Key.Contains(param.Query) ||
          type.Name.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(type => type.Id)
        .Paginate(param);
    }
  }
}
