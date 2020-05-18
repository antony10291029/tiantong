using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcRepository
  {
    public IotDbContext _db;

    public PlcRepository(IotDbContext db)
    {
      _db = db;
    }

    public void Add(Plc plc)
    {
      plc.id = 0;

      _db.Add(plc);
      _db.SaveChanges();
    }

    public void Delete(int id)
    {
      var plc = EnsureGet(id);
      _db.Plcs.Remove(plc);
      _db.SaveChanges();
    }

    public void Update(Plc plc)
    {
      var oldPlc = EnsureGet(plc.id);
      _db.Entry(oldPlc).CurrentValues.SetValues(plc);
      _db.Entry(oldPlc).Property(p => p.created_at).IsModified = false;
      _db.SaveChanges();
    }

    public Plc[] All()
    {
      return _db.Plcs
        .OrderBy(p => p.id)
        .ToArray();
    }

    public Plc[] AllWithRelationships()
    {
      return _db.Plcs
        .Include(p => p.states)
          .ThenInclude(s => s.state_http_pushers)
            .ThenInclude(shp => shp.pusher)
        .OrderBy(p => p.id)
        .ToArray();
    }

    public HttpPusher[] AllHttpPushers(int plcId)
    {
      System.Console.WriteLine(plcId);

      return _db.PlcStates
        .Include(s => s.state_http_pushers)
          .ThenInclude(shp => shp.pusher)
        .Where(s => s.plc_id == plcId)
        .ToArray()
        .SelectMany(s => s.http_pushers.Select(p => p))
        .ToArray();
    }

    public Pagination<HttpPusherLog> PaginateHttpPusherLogs(int[] ids, int page, int pageSize)
    {
      return _db.HttpPusherLogs
        .Where(hp => ids.Contains(hp.pusher_id))
        .OrderByDescending(hp => hp.id)
        .Paginate(page, pageSize);
    }

    public Pagination<HttpPusherError> PaginateHttpPusherErrors(int[] ids, int page, int pageSize)
    {
      return _db.HttpPusherErrors
        .Where(hp => ids.Contains(hp.pusher_id))
        .OrderByDescending(hp => hp.id)
        .Paginate(page, pageSize);
    }

    public Plc Get(int id)
    {
      return _db.Plcs.FirstOrDefault(p => p.id == id);
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
      return _db.Plcs
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
