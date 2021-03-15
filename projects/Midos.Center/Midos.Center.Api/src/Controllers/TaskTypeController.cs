using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class TaskTypeController: BaseController
  {
    private DomainContext _domain;

    public TaskTypeController(DomainContext domain)
    {
      _domain = domain;
    }

    public class CreateParams
    {
      public string Key { get; set; }

      public string Name { get; set; }

      public string Data { get; set; }

      public string Comment { get; set; }
    }

    public class CreateResult: MessageObject
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/types/create")]
    public INotifyResult<CreateResult> Create([FromBody] CreateParams param)
    {
      var type = TaskType.FromRequest(param);
      var result = new CreateResult();

      if (_domain.TaskTypes.Any(tp => tp.Key == type.Key)) {
        return NotifyResult
          .From(result)
          .Danger("任务类型编号已存在");
      }

      _domain.Add(type);
      _domain.SaveChanges();

      result.Id = result.Id;

      return NotifyResult
        .From(result)
        .Success("任务类型创建成功");
    }

    public class UpdateParams
    {
      public long Id { get; set; }

      public string Name { get; set; }

      public string Data { get; set; }

      public string Comment { get; set; }
    }

    [HttpPost("/midos/tas/types/update")]
    public INotifyResult<IMessageObject> Update([FromBody] UpdateParams param)
    {
      var type = _domain.TaskTypes.Find(param.Id);

      type.UpdateFromRequest(param);
      _domain.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务类型已更新");
    }

    public class DeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/types/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var type = _domain.TaskTypes.Find(param.Id);
      var types = _domain.SubtaskTypes.Where(st => st.TypeId == type.Id).ToArray();

      _domain.Remove(type);
      _domain.RemoveRange(types);

      return NotifyResult.FromVoid().Success("任务类型已删除");
    }

    [HttpPost("/midos/tas/types/search")]
    public object All()
    {
      return _domain.TaskTypes.ToArray();
    }
  }
}
