using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcRepository
  {
    public LogContext _log;

    public SystemContext _system;

    public PlcRepository(LogContext log, SystemContext system)
    {
      _log = log;
      _system = system;
    }

    public void Add(Plc plc)
    {
      plc.id = 0;

      if (_system.Plcs.Any(p => p.name == plc.name)) {
        throw new FailureOperation("设备名称已存在");
      }

      _system.Add(plc);
      _system.SaveChanges();
    }

    public void Delete(int id)
    {
      var plc = EnsureGet(id);
      _system.Plcs.Remove(plc);
      _system.SaveChanges();
    }

    public void Update(Plc plc)
    {
      var oldPlc = EnsureGet(plc.id);
      _system.Entry(oldPlc).CurrentValues.SetValues(plc);
      _system.Entry(oldPlc).Property(p => p.created_at).IsModified = false;
      _system.SaveChanges();
    }

    public Plc[] All()
    {
      return _system.Plcs
        .OrderBy(p => p.id)
        .ToArray();
    }

    public Plc[] AllWithRelationships()
    {
      return _system.Plcs
        .Include(p => p.states)
          .ThenInclude(s => s.state_http_pushers)
            .ThenInclude(shp => shp.pusher)
        .OrderBy(p => p.id)
        .ToArray();
    }

    public HttpPusher[] AllHttpPushers(int plcId)
    {
      System.Console.WriteLine(plcId);

      return _system.PlcStates
        .Include(s => s.state_http_pushers)
          .ThenInclude(shp => shp.pusher)
        .Where(s => s.plc_id == plcId)
        .ToArray()
        .SelectMany(s => s.http_pushers.Select(p => p))
        .ToArray();
    }

    public Pagination<HttpPusherLog> PaginateHttpPusherLogs(int[] ids, int page, int pageSize)
    {
      return _log.HttpPusherLogs
        .Where(hp => ids.Contains(hp.pusher_id))
        .OrderByDescending(hp => hp.id)
        .Paginate(page, pageSize);
    }

    public Pagination<HttpPusherError> PaginateHttpPusherErrors(int[] ids, int page, int pageSize)
    {
      return _log.HttpPusherErrors
        .Where(hp => ids.Contains(hp.pusher_id))
        .OrderByDescending(hp => hp.id)
        .Paginate(page, pageSize);
    }

    public Plc Get(int id)
    {
      return _system.Plcs.FirstOrDefault(p => p.id == id);
    }

    public Plc EnsureGet(int id)
    {
      var plc = Get(id);

      if (plc == null) {
        throw new FailureOperation("PLC 不存在");
      }

      return plc;
    }

    public Plc Find(int id)
    {
      return _system.Plcs
        .Include(p => p.states)
          .ThenInclude(s => s.state_http_pushers)
            .ThenInclude(shp => shp.pusher)
        .FirstOrDefault(p => p.id == id);
    }

    public Plc EnsureFind(int id)
    {
      var plc = Find(id);

      if (plc == null) {
        throw new FailureOperation("PLC 不存在");
      }

      return plc;
    }

  }

}
