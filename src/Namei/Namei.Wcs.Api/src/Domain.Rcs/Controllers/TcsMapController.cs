using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Aggregates
{
  [Controller]
  [Route("/rcsMap/")]
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

    [HttpPost("/toDataName")]
    public object ToDataName([FromBody] ToDataNamesParams param)
      => new { result = _rcs.ToDataName(param.Codes) };

    public class GetFreeLocationCodeParams
    {
      public string AreaCode { get; set; }
    }

    [HttpPost("/getFreeLocationCode")]
    public object GetFreeLocationCode([FromBody] GetFreeLocationCodeParams param)
      => new { result = _rcs.GetFreeLocationCode(param.AreaCode) };
  }
}
