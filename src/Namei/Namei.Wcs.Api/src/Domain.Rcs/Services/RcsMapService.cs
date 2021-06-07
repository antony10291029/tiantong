using System.Linq;
using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public interface IRcsMapService
  {
    string[] ToDataName(string[] codes);

    string GetFreeLocationCode(string areaCode);

    void UpdateRange(TcsMapData[] codes);

    object Search(string areaCode = null);

    bool IsTaskStartedWithPod(string podCode);

    bool HasTask(string taskCode);
  }

  public class RcsMapService: IRcsMapService
  {
    private readonly RcsContext _context;

    public RcsMapService(RcsContext context)
    {
      _context = context;
    }

    public bool IsTaskStartedWithPod(string podCode)
    {
      if (string.IsNullOrWhiteSpace(podCode)) {
        return false;
      }

      return  _context.Set<TcsMainTask>().Any(task =>
        task.PodCode == podCode && (
          task.TaskStatus == TcsMainTaskStatus.Created ||
          task.TaskStatus == TcsMainTaskStatus.Started
        )
      );
    }

    public string[] ToDataName(string[] codes)
    {
      var validCodes = codes.Where(code => code.Length > 6).ToArray();
      var locations = _context.Set<TcsMapData>()
        .Where(location => validCodes.Contains(location.MapDataCode)).ToArray()
        .ToDictionary(location => location.MapDataCode, location => location);

      return codes
        .Select(code => locations.ContainsKey(code) ? locations[code].DataName : code)
        .ToArray();
    }

    public string GetFreeLocationCode(string areaCode)
    {
      var locations = _context.Set<TcsMapData>()
        .Where(location => location.AreaCode == areaCode)
        .Where(location => location.PodCode == null)
        .ToArray();

      var destinations = _context.Set<TcsMainTask>()
        .Where(task => task.TaskStatus == TcsMainTaskStatus.Created || task.TaskStatus == TcsMainTaskStatus.Started)
        .ToArray()
        .Select(task => task.Destination)
        .Distinct();

      return locations
        .Where(location => !destinations.Contains(location.MapDataCode))
        .OrderBy(location => location.WcsAreaSeq)
        .ThenBy(location => location.DataName)
        .FirstOrDefault()?.DataName;
    }

    public void UpdateRange(TcsMapData[] data)
    {
      var codes = data.Select(map => map.MapDataCode);
      var dict = data.ToDictionary(data => data.MapDataCode, data => data);
      var source = _context.Set<TcsMapData>()
        .Where(map => codes.Contains(map.MapDataCode))
        .ToArray();

      foreach (var item in source) {
        item.WcsAreaSeq = dict[item.MapDataCode].WcsAreaSeq;
      }

      _context.SaveChanges();
    }

    public object Search(string areaCode = null)
    {
      var query = _context.Set<TcsMapData>().AsQueryable();

      if (!string.IsNullOrWhiteSpace(areaCode)) {
        query = query.Where(map => map.AreaCode == areaCode);
      }

      var data = query
        .OrderBy(map => map.AreaCode == null)
        .ThenByDescending(map => map.AreaCode)
        .ThenBy(map => map.WcsAreaSeq)
        .ThenByDescending(map => map.DataName)
        .ToArray();

      return new {
        Keys = data.Select(map => map.MapDataCode),
        Values = data.ToDictionary(map => map.MapDataCode, map => map)
      };
    }

    public bool HasTask(string taskCode)
      => _context.Set<TcsMainTask>()
        .Any(task => task.MainTaskNum == taskCode);
  }
}
