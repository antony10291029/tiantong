using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Midos.Domain;

namespace Namei.Wcs.Api
{
  public class LoggerController: BaseController
  {
    private DomainContext _domain;

    public LoggerController(DomainContext domain)
    {
      _domain = domain;
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
