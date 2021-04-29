using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;
using Midos.Services.Http;
using Namei.Wcs.Api;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgvTaskController
  {
    private readonly WcsContext _context;

    public RcsAgvTaskController(WcsContext context)
    {
      _context = context;
    }

    public struct CreateParams
    {
      public string Type { get; set; }

      public string TaskId { get; set; }

      public string Position { get; set; }

      public string Destination { get; set; }

      public string PalletCode { get; set; }

      public string AgvCode { get; set; }
    }

    public class CreateResponse: MessageObject
    {
      public int Code { get; set; }

      public object Data { get; set; }
    }

    [HttpPost("/agc-tasks/create")]
    public INotifyResult<CreateResponse> Create([FromBody] CreateParams param)
    {
      var res = new CreateResponse {
        Code = 0,
        Data = param
      };
      var result = NotifyResult.From(res);
      var type = _context.Set<AgcTaskType>()
        .FirstOrDefault(type => type.Key == param.Type);

      if (type is null) {
        result.Danger("任务类型不存在");

        return result;
      }

      result.Success("任务下发成功");

      return result;
    }
  }
}
