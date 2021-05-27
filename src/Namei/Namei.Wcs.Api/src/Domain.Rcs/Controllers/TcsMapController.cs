using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Aggregates
{
  [Controller]
  [Route("/rcs/mapData")]
  public class RcsController
  {
    private readonly IRcsMapService _rcs;

    public RcsController(IRcsMapService rcs)
    {
      _rcs = rcs;
    }

    public class ToDataNamesParams
    {
      public string[] Codes { get; set; }
    }

    [HttpPost("updateRange")]
    public object UpdateRange([FromBody] TcsMapData[] param)
    {
      _rcs.UpdateRange(param);

      return NotifyResult.FromVoid().Success("数据已更新");
    }

    [HttpPost("toDataName")]
    public object ToDataName([FromBody] ToDataNamesParams param)
      => new { result = _rcs.ToDataName(param.Codes) };

    public class GetFreeLocationCodeParams
    {
      public string AreaCode { get; set; }
    }

    [HttpPost("getFreeLocationCode")]
    public object GetFreeLocationCode([FromBody] GetFreeLocationCodeParams param)
      => new { result = _rcs.GetFreeLocationCode(param.AreaCode) };

    public class SearchParams
    {
      public string AreaCode { get; set; }
    }

    [HttpPost("search")]
    public object Search([FromBody] SearchParams param)
      => _rcs.Search(param.AreaCode);
  }
}
