using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class DoorTaskWebController: BaseController
  {
    private DomainContext _domain;

    public DoorTaskWebController(DomainContext domain)
    {
      _domain = domain;
    }

    [HttpPost("/door-tasks/search")]
    public IPagination<RcsDoorTask, string> Search([FromBody] QueryParams param)
    {
      var query = _domain.Set<RcsDoorTask>().AsQueryable();

      if (param.Query != null && param.Query != "") {
        query = query.Where(task =>
          task.Id.Contains(param.Query) ||
          task.DoorId.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(task => task.Status == RcsDoorTaskStatus.Requested)
        .ThenByDescending(task => task.Status == RcsDoorTaskStatus.Entered)
        .ThenByDescending(task => task.RequestedAt)
        .ThenByDescending(task => task.Id)
        .Paginate<RcsDoorTask, string>(param);
    }
  }
}
