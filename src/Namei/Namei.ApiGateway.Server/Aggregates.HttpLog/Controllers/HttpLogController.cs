using Microsoft.AspNetCore.Mvc;
using Midos.SeedWork.Domain;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Namei.ApiGateway.Server
{
  public class HttpLogController
  {
    private readonly AppContext _context;

    public HttpLogController(AppContext context)
    {
      _context = context;
    }

    public record SearchParams: PaginateParams
    {
      public string SourcePath { get; set; }
    }

    [HttpPost("/$http-logs/search")]
    public Pagination<HttpLog> Search([FromBody] SearchParams param)
    {
      var query = _context.Set<HttpLog>().AsQueryable();

      if (!string.IsNullOrWhiteSpace(param.SourcePath)) {
        query = query.Where(log => log.SourcePath == param.SourcePath);
      }

      if (!string.IsNullOrWhiteSpace(param.Query)) {
        query = query.Where(log =>
          log.RequestBody.Contains(param.Query) ||
          log.ResponseBody.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(log => log.Id)
        .Paginate(param);
    }

    public class ClearParams
    {
      public int Days { get; set; }
    }

    [HttpPost("/$http-logs/clear")]
    public INotifyResult<IMessageObject> Clear([FromBody] ClearParams param)
    {
      var date = DateTime.Now.AddDays(0 - param.Days);

      _context.Set<HttpLog>()
        .Where(log => log.RequestedAt < date)
        .Delete();

      return NotifyResult
        .FromVoid()
        .Success("日志数据已清楚");
    }
  }
}
