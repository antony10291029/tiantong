using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Namei.Common.Api
{
  public class RcsController: BaseController
  {
    const string LocationMethod = "location";

    const string AreaMethod = "area";

    static readonly Dictionary<string, bool> NotAllowedArea = new Dictionary<string, bool> {
      { "201", true },
      { "216", true },
      { "217", true },
      { "220", true },
      { "221", true },
      { "316", true },
      { "317", true },
      { "401", true },
    };

    private RcsContext _rcs;

    private RcsHttpService _rcsHttp;

    public RcsController(RcsContext rcs, RcsHttpService rcsHttp)
    {
      _rcs = rcs;
      _rcsHttp = rcsHttp;
    }

    public class SearchParams
    {
      public string MapCode { get; set; }

      public string MapDataCode { get; set; }

      public string AreaCode { get; set; }

      public string PodCode { get; set; }

      public string DataName { get; set; }

      public bool? IsPodBound { get; set; }

      public int Page { get; set; } = 1;

      public int PageSize { get; set; } = 20;
    }

    [HttpPost("/public/rcs/mapData/search")]
    public Pagination<MapData> Search([FromBody] SearchParams param)
    {
      var query = _rcs.MapData.AsQueryable();

      if (param.IsPodBound == true) {
        query = query.Where(map => map.PodCode != null);
      } else if (param.IsPodBound == false) {
        query = query.Where(map => map.PodCode == null);
      }

      if (param.MapCode != null) {
        query = query.Where(map => map.MapCode == param.MapCode);
      }

      if (param.AreaCode != null) {
        query = query.Where(map => map.AreaCode == param.AreaCode);
      }

      if (param.PodCode != null) {
        query = query.Where(map => map.PodCode == param.PodCode);
      }

      if (param.DataName != null) {
        query = query.Where(map => map.DataName == param.DataName);
      }

      return query.Paginate(param.Page, param.PageSize);
    }

    public class UnbindPodRequest
    {
      public string Method { get; set; }

      public string LocationCode { get; set; }
    }

    public class BindResult
    {
      public string MapDataCode { get; set; }

      public string DataName { get; set; }

      public string PodCode { get; set; }

      public string AreaCode { get; set; }

      public string Message { get; set; }

      public BindResult(string mapCode, string dataName, string podCode, string areaCode)
      {
        MapDataCode = mapCode;
        DataName = dataName;
        PodCode = podCode;
        AreaCode = areaCode;
      }
    }

    public class UnbindPodResult: Result
    {
      public List<BindResult> FailedTasks { get; set; } = new List<BindResult>();

      public List<BindResult> ExecutedTasks { get; set; } = new List<BindResult>();
    }

    [HttpPost("/public/rcs/unbindPodAndBerth")]
    public object UnbundlePod([FromBody] UnbindPodRequest param)
    {
      var result = new UnbindPodResult();
      var map = _rcs.MapData.FirstOrDefault(
        map => map.MapDataCode == param.LocationCode || map.DataName == param.LocationCode
      );

      result.Message = "解绑操作执行完毕";

      if (map == null) {
        result.SetError($"库位编号不存在: {param.LocationCode}", "10");

        return result;
      }

      var maps = new List<MapData>();

      if (param.Method == LocationMethod) {
        maps = _rcs.MapData.Where(item => item.PodCode == map.PodCode && item.PodCode != null).ToList();
      } else if (param.Method == AreaMethod) {
        if (map.AreaCode == null) {
          result.SetError("该库位不属于任何区域", "11");

          return result;
        } else if (NotAllowedArea.ContainsKey(map.AreaCode)) {
          result.SetError($"该区域不支持整体解绑: {param.LocationCode}", "12");

          return result;
        }

        maps = _rcs.MapData.Where(item => item.AreaCode == map.AreaCode && item.PodCode != null).ToList();
      } else {
        result.SetError($"解绑方式不支持: {param.Method}", "13");

        return result;
      }

      foreach (var item in maps) {
        var req = new BindPodAndBerthRquest(item.PodCode, item.MapDataCode, "0");
        var rcsResult = _rcsHttp.BindPodAndBerth(req).GetAwaiter().GetResult();
        var bindResult = new BindResult(item.MapDataCode, item.DataName, item.PodCode, item.AreaCode);

        if (rcsResult.code != "0") {
          bindResult.Message = rcsResult.message;
          result.FailedTasks.Add(bindResult);
          result.SetError("部分托架解绑失败，请查询系统核实", "30");
        } else {
          bindResult.Message = "解绑成功";
          result.ExecutedTasks.Add(bindResult);
        }
      }

      return result;
    }
  }
}
