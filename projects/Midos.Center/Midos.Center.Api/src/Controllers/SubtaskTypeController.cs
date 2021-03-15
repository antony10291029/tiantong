using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Center.Entities;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class SubtaskTypeController: BaseController
  {
    private DomainContext _domain;

    public SubtaskTypeController(DomainContext domain)
    {
      _domain = domain;
    }

    public class CreateParams: MessageObject
    {
      public string Key { get; set; }

      public int Index { get; set; }

      public long TypeId { get; set; }

      public long SubtypeId { get; set; }
    }

    public class CreateResult: MessageObject
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/subtypes/create")]
    public INotifyResult<CreateResult> Create([FromBody] CreateParams param)
    {
      var subtype = _domain.SubtaskTypes.FirstOrDefault(
        st => st.TypeId == param.TypeId && st.Key == param.Key
      );
      var result = new CreateResult();

      if (subtype != null) {
        return NotifyResult
          .From(result)
          .Danger("子任务类型编号已存在");
      } else {
        subtype = SubtaskType.FromRequest(param);
      }

      _domain.Add(subtype);
      _domain.SaveChanges();
      result.Id = subtype.Id;

      return NotifyResult
        .From(result)
        .Success("子任务类型已创建");
    }

    public class DeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/subtypes/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var subtask = _domain.SubtaskTypes.Find(param.Id);

      _domain.Remove(subtask);
      _domain.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("子任务类型已删除");
    }

    public class UpdateParams
    {
      public long Id { get; set; }

      public int Index { get; set; }

      public long SubtypeId { get; set; }
    }

    [HttpPost("/midos/tas/subtypes/update")]
    public INotifyResult<IMessageObject> Update([FromBody] UpdateParams[] param)
    {
      var ids = param.Select(st => st.Id);
      var types = _domain.SubtaskTypes
        .Where(st =>  ids.Contains(st.Id))
        .ToArray();

      foreach (var subtype in types) {
        subtype.UpdateFromRequest(param.First(p => p.Id == subtype.Id));
      }

      if (types.Length != 0) {
        _domain.SaveChanges();
      }

      return NotifyResult
        .FromVoid()
        .Success("任务类型修改成功");
    }

    public class SearchParams
    {
      public long TaskTypeId { get; set; }
    }

    [HttpPost("/midos/tas/subtypes/search")]
    public SubtaskType[] Search([FromBody] SearchParams param)
    {
      var subtypes = _domain.SubtaskTypes
        .Include(st => st.Subtype)
        .Where(st => st.TypeId == param.TaskTypeId)
        .ToArray();

      return subtypes;
    }
  }
}
