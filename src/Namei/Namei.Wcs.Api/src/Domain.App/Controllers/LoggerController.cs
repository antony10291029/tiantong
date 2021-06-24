using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Midos.Eventing;
using Midos.Services.Http;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LoggerController: BaseController
  {
    const string Group = "LoggerController";

    private readonly DomainContext _domain;

    private readonly Logger _logger;

    public LoggerController(
      DomainContext domain,
      Logger logger
    ) {
      _logger = logger;
      _domain = domain;
    }

    [EventSubscribe(HttpLogEvent.@event, Group)]
    public void HandleEvengLog(HttpLogEvent param)
    {
      _logger.Save(
        level: Log.UseSuccess(),
        klass: "http.service",
        operation: "post",
        message: "Http 请求执行完毕",
        index: "0",
        data: param
      );
    }

    public class SearchParams
    {
      public string[] Query { get; set; }

      public string[] Classes { get; set; }

      public int Page { get; set; }

      public int PageSize { get; set; } = 20;
    }

    [HttpPost]
    [Route("/logs/search")]
    public IPagination<Log> Search([FromBody] SearchParams param)
    {
      var queryBuilder = _domain.Logs.AsQueryable();

      foreach (var klass in param.Classes) {
        queryBuilder = queryBuilder.Where(log => log.Class == klass);
      }

      foreach (var query in param.Query) {
        queryBuilder = queryBuilder.Where(log =>
          log.Message.Contains(query) ||
          log.Data.Contains(query) ||
          log.Operation.Contains(query)
        );
      }

      return queryBuilder
        .OrderByDescending(log => log.CreatedAt)
        .ThenByDescending(log => log.Id)
        .PaginateNext(param.Page, param.PageSize);
    }
  }
}
