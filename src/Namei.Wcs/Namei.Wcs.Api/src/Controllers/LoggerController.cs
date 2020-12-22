using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;
using System.Linq;
using System.Collections.Generic;

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
      public List<string> search { get; set; } = new List<string>();

      public List<string> query { get; set; } = new List<string>();

      public int page { get; set; }

      public int page_size { get; set; } = 20;
    }

    [HttpPost]
    [Route("/logs/search")]
    public object Search([FromBody] SearchParams param)
    {
      var search = param.search;
      var searchCount = search.Count;
      var query = param.query;
      var queryCount = query.Count;

      if (searchCount > 10 || queryCount > 10) {
        throw KnownException.Error("查询参数超过最多为 10 个");
      }

      return _domain.Logs
        .Where(log =>
          (0 < searchCount ? log.key.Contains(search[0]) : true) &&
          (1 < searchCount ? log.key.Contains(search[1]) : true) &&
          (2 < searchCount ? log.key.Contains(search[2]) : true) &&
          (3 < searchCount ? log.key.Contains(search[3]) : true) &&
          (4 < searchCount ? log.key.Contains(search[4]) : true) &&
          (5 < searchCount ? log.key.Contains(search[5]) : true) &&
          (6 < searchCount ? log.key.Contains(search[6]) : true) &&
          (7 < searchCount ? log.key.Contains(search[7]) : true) &&
          (8 < searchCount ? log.key.Contains(search[8]) : true) &&
          (9 < searchCount ? log.key.Contains(search[9]) : true) &&
          (10 < searchCount ? log.key.Contains(search[10]) : true) &&
          (0 < queryCount ? log.message.Contains(query[0]) : true) &&
          (1 < queryCount ? log.message.Contains(query[1]) : true) &&
          (2 < queryCount ? log.message.Contains(query[2]) : true) &&
          (3 < queryCount ? log.message.Contains(query[3]) : true) &&
          (4 < queryCount ? log.message.Contains(query[4]) : true) &&
          (5 < queryCount ? log.message.Contains(query[5]) : true) &&
          (6 < queryCount ? log.message.Contains(query[6]) : true) &&
          (7 < queryCount ? log.message.Contains(query[7]) : true) &&
          (8 < queryCount ? log.message.Contains(query[8]) : true) &&
          (9 < queryCount ? log.message.Contains(query[9]) : true) &&
          (10 < queryCount ? log.message.Contains(query[10]) : true)
        )
        .OrderByDescending(log => log.created_at)
        .ThenByDescending(log => log.id)
        .Paginate(param.page, param.page_size);
    }
  }
}
