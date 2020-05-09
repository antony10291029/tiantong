using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcStateHttpPusherRepository
  {
    private IotDbContext _db;

    public PlcStateHttpPusherRepository(IotDbContext db)
    {
      _db = db;
    }

    public void Add(int stateId, HttpPusher pusher)
    {
      var statePusher = new PlcStateHttpPusher {
        state_id = stateId,
        pusher = pusher,
      };

      _db.Add(statePusher);
      _db.SaveChanges();
    }

    public void Delete(int stateId, int pusherId)
    {
      var statePusher = _db.PlcStateHttpPushers
        .Include(sp => sp.pusher)
        .FirstOrDefault(sp => sp.state_id == stateId && sp.pusher_id == pusherId);

      if (statePusher == null) {
        throw new FailureOperation("PLC 数据点不存在");
      }

      _db.Remove(statePusher);
      _db.SaveChanges();
    }

    public void Update(int stateId, HttpPusher pusher)
    {
      var oldPusher = _db.HttpPushers.FirstOrDefault(s => s.id == pusher.id);

      if (oldPusher == null) {
        throw new FailureOperation("HTTP推送不存在");
      }

      _db.Entry(oldPusher).CurrentValues.SetValues(pusher);
      _db.SaveChanges();
    }

    public HttpPusher[] All(int stateId)
    {
      return _db.PlcStateHttpPushers
        .Include(state => state.pusher)
        .Where(shp => shp.state_id == stateId)
        .Select(shp => shp.pusher)
        .ToArray();
    }

  }
}
