using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Aggregates;
using Midos.Domain;
using System.Collections.Generic;
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

    public class TaskTypeParams
    {
      public long Id { get; set; }

      public string Key { get; set; }

      public string Name { get; set; }

      public bool HasCode { get; set; }

      public Record Data { get; set; }

      public string Comment { get; set; }

      public List<SubtaskTypeParams> Subtypes { get; set; }
    }

    public class SubtaskTypeParams
    {
      public long Id { get; set; }

      public int Index { get; set; }

      public string Key { get; set; }

      public long TypeId { get; set; }

      public long SubtypeId { get; set; }
    }

    public class CreateResult: MessageObject
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/types/create")]
    public INotifyResult<CreateResult> Create([FromBody] TaskTypeParams param)
    {
      var data = new CreateResult();
      var type = TaskType.From(param);
      var result = NotifyResult.From(data);
      var subkeys = type.Subtypes.Select(st => st.Key).ToArray();
      var subids = type.Subtypes.Select(st => st.SubtypeId).ToArray();

      foreach (var subtask in param.Subtypes) {
        if (subtask.SubtypeId == 0) {
          return result.Danger("子任务不可为空");
        }
      }

      if (subkeys.Distinct().ToArray().Length != subkeys.Length) {
        return result.Danger("子任务类型编号不可重复");
      }

      if (_domain.Set<TaskType>().Any(tp => tp.Key == type.Key)) {
        return result.Danger("任务类型编号已存在");
      }

      _domain.Add(type);
      _domain.SaveChanges();

      data.Id = type.Id;

      return result.Success("任务类型创建成功");
    }

    [HttpPost("/midos/tas/types/update")]
    public INotifyResult<IMessageObject> Update([FromBody] TaskTypeParams param)
    {
      var result = NotifyResult.FromVoid();
      var type = _domain.Set<TaskType>()
        .Include(type => type.Subtypes)
        .First(type => type.Id == param.Id);

      foreach (var subtask in param.Subtypes) {
        if (subtask.SubtypeId == 0) {
          return result.Danger("子任务不可为空");
        }
      }

      _domain.RemoveRange(type.Update(param));
      _domain.SaveChanges();

      return result.Success("任务类型已更新");
    }

    public class DeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/types/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var type = _domain.Set<TaskType>()
        .Include(type => type.Subtypes)
        .First(type => type.Id == param.Id);

      _domain.Remove(type);
      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("任务类型已删除");
    }

    [HttpPost("/midos/tas/types/search")]
    public IDataMap<TaskType> All()
    {
      return _domain.Set<TaskType>()
        .Include(type => type.Subtypes)
        .OrderByDescending(type => type.Id)
        .ToDataMap();
    }
  }
}
