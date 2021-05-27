using System.Linq;

namespace Namei.Wcs.Aggregates
{
  public interface IRcsMapService
  {
    string[] ToDataName(string[] codes);

    string GetFreeLocationCode(string areaCode);
  }

  public class RcsMapService: IRcsMapService
  {
    private readonly RcsContext _context;

    public RcsMapService(RcsContext context)
    {
      _context = context;
    }

    private TcsMainTask[] GetTasks(string status = null)
    {
      var query = _context.Set<TcsMainTask>().AsQueryable();

      if (status != null) {
        query = query.Where(task => task.TaskStatus == status);
      }

      return query.ToArray();
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
      var destinations = GetTasks(TcsMainTaskStatus.Started)
        .Select(task => task.Destination)
        .Distinct();

      var locations = _context.Set<TcsMapData>()
        .Where(location => location.AreaCode == areaCode)
        .Where(location => location.PodCode == null)
        .ToArray();

      return locations
        .Where(location => !destinations.Contains(location.MapDataCode))
        .OrderByDescending(location => location.WcsAreaSeq)
        .FirstOrDefault()?.DataName;
    }
  }
}
