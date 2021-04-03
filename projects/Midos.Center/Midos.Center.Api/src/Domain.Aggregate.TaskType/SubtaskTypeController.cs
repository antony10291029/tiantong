using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using Microsoft.EntityFrameworkCore;
using Midos.Domain;
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

    public class DeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/tas/subtypes/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] DeleteParams param)
    {
      var subtask = _domain.Find<SubtaskType>(param.Id);

      _domain.Remove(subtask);
      _domain.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("子任务类型已删除");
    }

    public class SearchParams
    {
      public long TaskTypeId { get; set; }
    }

    [HttpPost("/midos/tas/subtypes/search")]
    public SubtaskType[] Search([FromBody] SearchParams param)
    {
      var subtypes = _domain.Set<SubtaskType>()
        .Include(st => st.Subtype)
        .Where(st => st.TypeId == param.TaskTypeId)
        .ToArray();

      return subtypes;
    }
  }
}
