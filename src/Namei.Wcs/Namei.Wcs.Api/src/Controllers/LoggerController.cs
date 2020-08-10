using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;
using System.Linq;

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
      public string search { get; set; }

      public int page { get; set; }

      public int page_size { get; set; } = 20;
    }

    [HttpPost]
    [Route("/logs/search")]
    public object Search([FromBody] SearchParams param)
    {
      return _domain.Logs.Where(log => log.key.StartsWith(param.search))
        .OrderByDescending(log => log.created_at)
        .Paginate(param.page, param.page_size);
    }
  }
}
