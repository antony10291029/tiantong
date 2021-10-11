using Microsoft.AspNetCore.Mvc;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/plcConfigs")]
  public class PlcConfigController
  {
    private readonly PlcConfigContext _context;

    public PlcConfigController(PlcConfigContext context)
    {
      _context = context;
    }

    public record GetParams
    {
      public int Id { get; set; }
    }

    [HttpPost("get")]
    public Plc GetPlc([FromBody] GetParams param)
    {
      return _context.Get(param.Id);
    }

    [HttpPost("update")]
    public object UpdatePlc([FromBody] Plc plc)
    {
      _context.Update(plc);

      return NotifyResult.FromVoid().Success("配置信息已更新");
    }
  }
}
