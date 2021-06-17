using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterTaskWebController: BaseController
  {
    private readonly WcsContext _context;

    public LifterTaskWebController(WcsContext context)
    {
      _context = context;
    }

    public class CloseParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/lifter-tasks/close")]
    public INotifyResult<IMessageObject> UpdateStatus([FromBody] CloseParams param)
    {
      var task = _context.Set<LifterTask>().Find(param.Id);
      var result = NotifyResult.FromVoid();

      if (task == null) {
        result.Danger("任务 Id 不存在");
      } else {
        task.Close();
        _context.SaveChanges();
        result.Success("任务已关闭");
      }

      return result;
    }

    [HttpPost("/lifter-tasks/search")]
    public IPagination<LifterTask> SearchTasks([FromBody] QueryParams param)
    {
      var query =  _context.Set<LifterTask>().AsQueryable();

      if (param.Query != null && param.Query != "") {
        query = query.Where(task =>
          task.Barcode.Contains(param.Query) ||
          task.TaskCode.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(task => task.Status == LifterTaskStatus.Exported)
        .ThenByDescending(task => task.Status == LifterTaskStatus.Imported)
        .ThenByDescending(task => task.ImportedAt)
        .Paginate(param);
    }
  }
}
